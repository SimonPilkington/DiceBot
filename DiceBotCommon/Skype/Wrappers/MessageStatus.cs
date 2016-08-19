namespace DiceBot.Skype.Wrappers
{
	public enum MessageStatus
	{
		Unknown = -1,
		Sending,
		Sent,
		Received,
		Read
	}

	public static class MessageStatusMethods
	{
		public static MessageStatus ToWrappedType(this SKYPE4COMLib.TChatMessageStatus status)
		{
			return (MessageStatus)status;
		}
	}
}
