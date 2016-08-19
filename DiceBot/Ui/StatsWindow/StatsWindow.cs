using DiceBot.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DiceBot.Ui
{
	public partial class StatsWindow : Form, IStatsView
	{
		private static StatsWindow statsWindow; // There should only be one of these at a time.
		private static StatsPresenter presenter; // Must store this reference somewhere or the presenter could get collected.
		public static void ShowStatsWindow(IReadDieStats stats)
		{
			if (statsWindow == null || statsWindow.IsDisposed)
			{
				statsWindow = new StatsWindow();
				statsWindow.Show();

				presenter = new StatsPresenter(statsWindow, stats);
			}
			else
				statsWindow.BringToFront();
		}

		private int dieForHistogram;

		private StatsWindow()
		{
			InitializeComponent();
		}

		private void datePicker_ValueChanged(object sender, EventArgs e)
		{
			var me = (DateTimePicker)sender;

			if (me.Value > (DateTime.Now - new TimeSpan(0, 59, 0)))
			{
				me.Value = new DateTime(
					DateTime.Now.Year,
					DateTime.Now.Month,
					DateTime.Now.Day,
					DateTime.Now.Hour - 1,
					0,
					0);
			}
		}

		private void StatsWindow_Load(object sender, EventArgs e)
		{
			datePicker.Value = new DateTime(
				datePicker.Value.Year,
				datePicker.Value.Month,
				datePicker.Value.Day,
				datePicker.Value.Hour,
				0,
				0) - new TimeSpan(1, 0, 0);

			datePicker.Checked = false;
		}

		private void statsDataGrid_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
		{
			var me = (DataGridView)sender;

			if (e.ColumnIndex != 0)
			{
				me.ContextMenuStrip = null;
				return;
			}

			var data = (DataTable)me.DataSource;

			if (data == null)
				return;

			dieForHistogram = (int)(long)(data.Rows[e.RowIndex][0]);
			me.ContextMenuStrip = dieRowStrip;
		}

		public void SetDisplayData(DataSet data)
		{
			if (data.Tables.Count == 0)
				throw new ArgumentException("Data set contains no tables.", nameof(data));

			statsDataGrid.DataSource = data.Tables[0];
		}

		public void SetUsers(IList<string> users)
		{
			usersComboBox.DataSource = users;
		}

		public bool CutoffDateEnabled => datePicker.Checked;
		public DateTime CutoffDate => datePicker.Value;
		public string SelectedUser => (string)usersComboBox.SelectedValue;
		
		public event EventHandler CutoffDateEnabledChanged
		{
			add { datePicker.EnabledChanged += value; }
			remove { datePicker.EnabledChanged -= value; }
		}

		public event EventHandler CutoffDateChanged
		{
			add { datePicker.ValueChanged += value; }
			remove { datePicker.ValueChanged -= value; }
		}

		public event EventHandler SelectedUserChanged
		{
			add { usersComboBox.SelectedValueChanged += value; }
			remove { usersComboBox.SelectedValueChanged -= value; }
		}

		public event EventHandler UserDieHistogramRequested
		{
			add { viewUserHistogramToolStripMenuItem.Click += value; }
			remove { viewUserHistogramToolStripMenuItem.Click -= value; }
		}

		public event EventHandler<HistogramRequestedEventArgs> DieHistogramRequested;

		private void viewUserHistogramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DieHistogramRequested?.Invoke(sender,
				new HistogramRequestedEventArgs(dieForHistogram, (string)usersComboBox.SelectedValue));
		}

		private void viewHistogramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DieHistogramRequested?.Invoke(sender,
				new HistogramRequestedEventArgs(dieForHistogram, null));
		}
	}
}
