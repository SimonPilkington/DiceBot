using System;

namespace DiceBot.Skype
{
	public class ContactRequestAcceptedEventArgs : EventArgs
	{
		public string UserName { get; }

		public ContactRequestAcceptedEventArgs(string userName)
		{
			UserName = userName;
		}
	}
}
