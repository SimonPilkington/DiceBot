using DiceBot.Statistics;
using System;
using System.Windows.Forms;

namespace DiceBot.Ui
{
	public partial class HistogramDialog : Form
	{
		// No MVP for this; too simple.

		private string userHandle;
		private int die;

		private IReadDieStats database;

		private readonly string seriesName;

		public HistogramDialog(int die, IReadDieStats database) : this(null, die, database)
		{	}

		public HistogramDialog(string userHandle, int die, IReadDieStats database)
		{
			InitializeComponent();

			this.userHandle = userHandle;
			this.die = die;
			this.database = database;

			seriesName = $"d{die} distribution";

			if (userHandle != null)
				seriesName += $" for {userHandle}";
		}

		private void HistogramWindow_Load(object sender, EventArgs e)
		{
			histogram.Series.Add(seriesName);
			for (int rollValue = 1; rollValue <= die; rollValue++)
			{
				long rolls = database.GetTotalRolls(userHandle, die, rollValue);
				int index = histogram.Series[seriesName].Points.AddXY(rollValue.ToString(), rolls);

				if (rolls > 0)
					histogram.Series[seriesName].Points[index].Label = rolls.ToString();
			}
		}
	}
}
