using System;

namespace DiceBot.Skype
{
	public class SlapAttemptedEventArgs : EventArgs
	{
		public SkypeBotCommandHandler.SlapResult Result { get; }
		public string UserName { get; }
		public string TargetName { get; }
		public string ChatName { get; }

		public SlapAttemptedEventArgs(string user, string target, string chatName, SkypeBotCommandHandler.SlapResult result)
		{
			UserName = user;
			TargetName = target;
			Result = result;
			ChatName = chatName;
		}
	}
}
