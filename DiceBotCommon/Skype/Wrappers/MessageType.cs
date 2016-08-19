namespace DiceBot.Skype.Wrappers
{
	public enum MessageType
	{
		Unknown = -1,
		CreatedChatWith,
		SawMembers,
		AddedMembers,
		SetTopic,
		Said,
		Left,
		Emoted,
		PostedContacts,
		GapInChat,
		SetRole,
		Kicked,
		SetOptions,
		KickBanned,
		JoinedAsApplicant,
		SetPicture,
		SetGuidelines
	}

	public static class MessageTypeMethods
	{
		public static MessageType ToWrappedType(this SKYPE4COMLib.TChatMessageType type)
		{
			return (MessageType)type;
		}
	}
}
