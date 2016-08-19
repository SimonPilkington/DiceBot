using DiceBot.Skype.Wrappers;
using System;

namespace DiceBot.Skype
{
	public class SkypeMessageReceivedEventArgs : EventArgs
	{
		public ISkypeMessage Message { get; }

		public bool CancelSetSeen { get; set; }

		public SkypeMessageReceivedEventArgs (ISkypeMessage message)
		{
			CancelSetSeen = false;
			Message = message;
		}
	}
}
