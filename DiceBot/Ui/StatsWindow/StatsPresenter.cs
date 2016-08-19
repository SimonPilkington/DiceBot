using DiceBot.Statistics;
using System;
using System.Data;
using System.Linq;

namespace DiceBot.Ui
{
	public class StatsPresenter
	{
		private IStatsView statsView;
		private IReadDieStats database;

		public StatsPresenter(IStatsView view, IReadDieStats statsDatabase)
		{
			statsView = view;
			database = statsDatabase;

			var users = statsDatabase.GetUids().Select(x => x.Key).ToArray();
			statsView.SetUsers(users);

			SetupEvents();

			FillForSelectedUser();
		}

		private void SetupEvents()
		{
			statsView.SelectedUserChanged += Refresh;
			statsView.CutoffDateEnabledChanged += Refresh;
			statsView.CutoffDateChanged += Refresh;

			statsView.DieHistogramRequested += HistogramRequested;
		}

		private void FillForSelectedUser()
		{
			if (statsView.SelectedUser == null) // This should only happen if the database is empty.
				return;

			// This should never return null because we got the list of handles that these uids are tied to from the database itself
			long? uid = database.GetUid(statsView.SelectedUser);
			var data = new DataSet();

			if (statsView.CutoffDateEnabled)
				database.FillStatsForUid(uid.Value, data, statsView.CutoffDate); // Cutoff date is always valid
			else
				database.FillStatsForUid(uid.Value, data);

			statsView.SetDisplayData(data);
		}

		private void Refresh(object sender, EventArgs e) => FillForSelectedUser();

		private void HistogramRequested(object sender, HistogramRequestedEventArgs e)
		{
			HistogramDialog window =
				e.UserHandle == null ? 
				new HistogramDialog(e.Die, database) :
				new HistogramDialog(e.UserHandle, e.Die, database);

			window.ShowDialog();
		}
	}
}
