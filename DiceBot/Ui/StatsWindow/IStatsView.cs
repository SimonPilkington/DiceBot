using System;
using System.Collections.Generic;
using System.Data;

namespace DiceBot.Ui
{
	public interface IStatsView
	{
		event EventHandler CutoffDateEnabledChanged;
		event EventHandler CutoffDateChanged;
		event EventHandler SelectedUserChanged;
		event EventHandler<HistogramRequestedEventArgs> DieHistogramRequested;

		bool CutoffDateEnabled { get; }
		DateTime CutoffDate { get; }
		string SelectedUser { get; }

		void SetDisplayData(DataSet data);
		void SetUsers(IList<string> users);
	}
}
