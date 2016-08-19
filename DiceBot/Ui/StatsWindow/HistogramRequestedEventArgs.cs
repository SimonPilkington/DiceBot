using System;

namespace DiceBot.Ui
{
	public class HistogramRequestedEventArgs : EventArgs
	{
		public int Die { get; }
		public string UserHandle { get; }

		public HistogramRequestedEventArgs(int die, string userHandle)
		{
			Die = die;
			UserHandle = userHandle;
		}
	}
}
