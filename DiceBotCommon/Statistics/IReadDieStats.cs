using System;
using System.Collections.Generic;

namespace DiceBot.Statistics
{
	public interface IReadDieStats
	{
		IEnumerable<KeyValuePair<string, long>> GetUids();
		IEnumerable<StatsRow> GetStatsForUid(long uid, DateTime since);

		void FillStatsForUid(long uid, System.Data.DataSet data, DateTime since);
		void FillStatsForUid(long uid, System.Data.DataSet data);

		long? GetUid(string userHandle);

		long GetTotalRolls(string userHandle, int die, int value);
		long GetTotalRolls(int die, int value);
		long GetTotalRolls(string userHandle, int die);
	}
}
