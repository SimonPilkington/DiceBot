using System;

namespace DiceBot.Skype
{
	public class GroupCreatedEventArgs : EventArgs
	{
		public string Requester { get; }

		public GroupCreatedEventArgs(string requester)
		{
			Requester = requester;
		}
	}
}
