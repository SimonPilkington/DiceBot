using System;
using System.Collections.Generic;

using System.Data.SQLite;
using System.Data.Common;

namespace DiceBot.Statistics
{
	public sealed class StatsDatabase : IDieStats, IReadDieStats, IDisposable
	{
		SQLiteConnection connection = new SQLiteConnection();

		public StatsDatabase()
		{
			SQLiteConnectionStringBuilder connString = new SQLiteConnectionStringBuilder();

			connString.DataSource = "stats.db";
			connString.ForeignKeys = true;

			connection.ConnectionString = connString.ConnectionString;

			connection.Open();
			InitTables();
		}

		private void InitTables()
		{
			using (DbTransaction transaction = connection.BeginTransaction())
			{
				using (SQLiteCommand cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"CREATE TABLE IF NOT EXISTS user(
										uid INTEGER PRIMARY KEY,
										handle TEXT UNIQUE NOT NULL);";

					cmd.ExecuteNonQuery();

					cmd.CommandText = @"CREATE TABLE IF NOT EXISTS roll(
														uid INTEGER NOT NULL REFERENCES user(uid),
														die INTEGER NOT NULL CHECK(die > 1),
														value INTEGER NOT NULL CHECK(value > 0 AND value <= die),
														time BIGINT NOT NULL);";

					cmd.ExecuteNonQuery();
				}

				transaction.Commit();
			}
		}

		public void RecordRoll(string userHandle, int die, int value)
		{
			if (userHandle == null)
				throw new ArgumentNullException(nameof(userHandle));

			long uid = GetOrAddUid(userHandle);

			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"INSERT INTO roll VALUES(:uid, :die, :value, :time);";
				cmd.Parameters.AddWithValue("uid", uid);
				cmd.Parameters.AddWithValue("die", die);
				cmd.Parameters.AddWithValue("value", value);
				cmd.Parameters.AddWithValue("time", DateTime.UtcNow.ToUnixTime());

				cmd.ExecuteNonQuery();
			}
		}

		public void RecordRollBatch(string userHandle, int die, IEnumerable<int> values)
		{
			if (userHandle == null)
				throw new ArgumentNullException(nameof(userHandle));

			long uid = GetOrAddUid(userHandle);

			using (var transaction = connection.BeginTransaction())
			{
				using (var cmd = connection.CreateCommand())
				{
					var valueParam = new SQLiteParameter("value");

					cmd.CommandText = @"INSERT INTO roll VALUES(:uid, :die, :value, :time);";
					cmd.Parameters.AddWithValue("uid", uid);
					cmd.Parameters.AddWithValue("die", die);
					cmd.Parameters.AddWithValue("time", DateTime.UtcNow.ToUnixTime());
					cmd.Parameters.Add(valueParam);

					foreach (int value in values)
					{
						valueParam.Value = value;
						cmd.ExecuteNonQuery();
					}
				}

				transaction.Commit();
			}
		}

		public long? GetUid(string userHandle)
		{
			if (userHandle == null)
				throw new ArgumentNullException(nameof(userHandle));

			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT uid FROM user WHERE user.handle = :userHandle;";
				cmd.Parameters.AddWithValue("userHandle", userHandle);

				object uidObject = cmd.ExecuteScalar();
				return (long?)uidObject;
			}
		}

		private long GetOrAddUid(string userHandle)
		{
			long? uid = GetUid(userHandle);

			return uid == null ? AddUser(userHandle) : uid.Value;
		}

		private long AddUser(string userHandle)
		{
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"INSERT INTO user(handle) VALUES(:userHandle);";
				cmd.Parameters.AddWithValue("userHandle", userHandle);

				cmd.ExecuteNonQuery();
			}

			return connection.LastInsertRowId;
		}

		public IEnumerable<KeyValuePair<string, long>> GetUids()
		{
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT uid, handle FROM user";

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var uid = reader.GetInt64(0);
						var handle = reader.GetString(1);

						yield return new KeyValuePair<string, long>(handle, uid);
					}
				}
			}
		}

		public IEnumerable<StatsRow> GetStatsForUid(long uid)
		{
			return GetStatsForUid(uid, DateTime.MinValue);
		}

		public IEnumerable<StatsRow> GetStatsForUid(long uid, DateTime since)
		{
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT die, AVG(value), AVG(value*value), COUNT(*), 
									(SELECT COUNT(*) FROM roll AS natOne WHERE natOne.uid = :uid AND natOne.die = roll.die AND value = 1 AND time > :offsetTime)
									FROM roll WHERE uid = :uid AND time > :offsetTime GROUP BY die;";

				cmd.Parameters.AddWithValue("uid", uid);

				if (since != DateTime.MinValue)
				{
					long unixTime = since.ToUnixTime();
					cmd.Parameters.AddWithValue("offsetTime", unixTime);
				}
				else
					cmd.Parameters.AddWithValue("offsetTime", 0);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						int die = reader.GetInt32(0);

						double mean = reader.GetDouble(1);
						double meanOfSquares = reader.GetDouble(2);
						double count = reader.GetDouble(3);
						int natOneCount = reader.GetInt32(4);

						double stdDev = Math.Sqrt((count / Math.Max(1, (count - 1))) * (meanOfSquares - mean * mean));

						yield return new StatsRow(die, mean, stdDev, natOneCount);
					}
				}
			}
		}

		public void FillStatsForUid(long uid, System.Data.DataSet data)
		{
			FillStatsForUid(uid, data, DateTime.MinValue);
		}

		public void FillStatsForUid(long uid, System.Data.DataSet data, DateTime since)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT die AS Die, AVG(value) AS Mean, AVG(value*value) As MeanOfSquares, COUNT(*) AS TotalRolled,
									(SELECT COUNT(*) FROM roll AS natOne WHERE natOne.uid = roll.uid AND natOne.die = roll.die AND natOne.value = 1 AND time > :offsetTime) AS NaturalOnes
									FROM roll WHERE uid = :uid AND time > :offsetTime GROUP BY die;";

				cmd.Parameters.AddWithValue("uid", uid);

				if (since != DateTime.MinValue)
				{
					long unixTime = since.ToUnixTime();
					cmd.Parameters.AddWithValue("offsetTime", unixTime);
				}
				else
					cmd.Parameters.AddWithValue("offsetTime", 0);

				using (var adapter = new SQLiteDataAdapter(cmd))
				{
					adapter.Fill(data);

					data.Tables[0].Columns[2].ColumnName = "StdDev";
					foreach (System.Data.DataRow row in data.Tables[0].Rows)
					{
						double mean = (double)row[1];
						double squaresMean = (double)row[2];
						double count = (long)row[3];

						row[2] = Math.Sqrt(count / Math.Max(1, (count - 1)) * squaresMean - mean * mean);
					}
				}
			}
		}
		
		const string basicRollCountCommand = @"SELECT COUNT(*) FROM roll WHERE die = :die";
		public long GetTotalRolls(string userHandle, int die)
		{
			if (userHandle == null)
				throw new ArgumentNullException(nameof(userHandle));

			return GetTotalRollsInternal(userHandle, die, 0);
		}

		public long GetTotalRolls(int die, int value)
		{
			if (value < 1 || value > die)
				throw new ArgumentOutOfRangeException(nameof(value));

			return GetTotalRollsInternal(null, die, value);
		}

		public long GetTotalRolls(string userHandle, int die, int value)
		{
			if (userHandle == null)
				throw new ArgumentNullException(nameof(userHandle));

			if (value < 1 || value > die)
				throw new ArgumentOutOfRangeException(nameof(value));

			return GetTotalRollsInternal(userHandle, die, value);
		}

		private long GetTotalRollsInternal(string userHandle, int die, int value)
		{
			if (die < 2)
				throw new ArgumentOutOfRangeException(nameof(die));

			long count = 0;
			using (SQLiteCommand cmd = connection.CreateCommand())
			{
				cmd.CommandText = basicRollCountCommand;

				if (value > 0)
				{
					cmd.CommandText += " AND value = :value";
					cmd.Parameters.AddWithValue("value", value);
				}

				if (userHandle != null)
				{
					long? uid = GetUid(userHandle);
					if (uid != null)
					{
						cmd.CommandText += " AND uid = :uid";
						cmd.Parameters.AddWithValue("uid", uid.Value);
					}
				}
				
				cmd.CommandText += ";";
				cmd.Parameters.AddWithValue("die", die);

				object result = cmd.ExecuteScalar();
				count = (long)result;
			}

			return count;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
				connection.Dispose();
		}
	}
}
