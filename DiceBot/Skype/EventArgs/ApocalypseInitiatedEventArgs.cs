using System;

namespace DiceBot.Skype
{
	public class ApocalypseInitiatedEventArgs : EventArgs
	{
		public string Initiator { get; }
		public string ChatName { get; }

		public ApocalypseInitiatedEventArgs(string initiator, string chatName)
		{
			Initiator = initiator;
			ChatName = chatName;
		}
	}
}
