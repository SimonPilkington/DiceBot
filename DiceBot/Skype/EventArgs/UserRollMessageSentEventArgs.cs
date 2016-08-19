using System;

namespace DiceBot.Skype
{
	public class UserRollMessageSentEventArgs : EventArgs
	{
		public string User { get; }
		public string Message { get; }

		public UserRollMessageSentEventArgs(string user, string message)
		{
			User = user;
			Message = message;
		}
	}
}
