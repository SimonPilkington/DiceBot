namespace DiceBot.Skype.Wrappers
{
	public enum ChatRole
	{
		Unknown = -1,
		Creator,
		Master,
		Helper,
		User,
		Listener,
		Applicant
	}

	public static class ChatRoleMethods
	{
		public static ChatRole ToWrappedChatRole(this SKYPE4COMLib.TChatMemberRole role)
		{
			return (ChatRole)role;
		}
	}
}
