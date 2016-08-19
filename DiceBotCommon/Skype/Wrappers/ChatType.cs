namespace DiceBot.Skype.Wrappers
{
	public enum ChatType
	{
		Unknown = -1,
		Dialog = 0,
		Group = 3
	}

	public static class ChatTypeMethods
	{
		public static ChatType ToWrappedChatType(this SKYPE4COMLib.TChatType type)
		{
			if (type == SKYPE4COMLib.TChatType.chatTypeDialog || type == SKYPE4COMLib.TChatType.chatTypeMultiChat)
				return (ChatType)type;

			return ChatType.Unknown;
		}
	}
}
