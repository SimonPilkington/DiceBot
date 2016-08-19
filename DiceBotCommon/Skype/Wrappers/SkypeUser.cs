using System;

namespace DiceBot.Skype.Wrappers
{
	public class SkypeUser : ISkypeUser
	{
		private SKYPE4COMLib.User _wrappedUser;

		public SKYPE4COMLib.User WrappedObject => _wrappedUser;

		public SkypeUser(SKYPE4COMLib.User wrappedUserObject)
		{
			if (wrappedUserObject == null)
				throw new ArgumentNullException(nameof(wrappedUserObject));

			_wrappedUser = wrappedUserObject;
		}

		public string FullName => _wrappedUser.FullName;

		public string Handle => _wrappedUser.Handle;

		public bool IsContact => _wrappedUser.IsAuthorized;
	}
}
