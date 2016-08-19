using SKYPE4COMLib;
using System;
using System.Collections.Generic;

namespace DiceBot.Skype.Wrappers
{
	public class SkypeChat : ISkypeChat
	{
		private Chat _wrappedChat;

		public string Guid => _wrappedChat.Blob;
		public string Name => _wrappedChat.FriendlyName;
		public string Topic
		{
			get { return _wrappedChat.Topic; }
			set { _wrappedChat.Topic = value; }
		}

		public ChatType Type => _wrappedChat.Type.ToWrappedChatType();
		public ChatRole MyRole => _wrappedChat.MyRole.ToWrappedChatRole();

		public IEnumerable<ISkypeUser> GetMembers()
		{
			foreach (User user in _wrappedChat.ActiveMembers)
				yield return new SkypeUser(user);
		}

		public SkypeChat(Chat wrappedChatObject)
		{
			if (wrappedChatObject == null)
				throw new ArgumentNullException(nameof(wrappedChatObject));

			_wrappedChat = wrappedChatObject;
		}

		public ChatRole GetRole(ISkypeUser user)
		{
			return GetRole(user.Handle);
		}

		public ChatRole GetRole(string handle)
		{
			foreach (IChatMember member in _wrappedChat.MemberObjects)
			{
				if (member.Handle == handle)
					return member.Role.ToWrappedChatRole();
			}

			throw new ArgumentException($"User {handle} is not in the chat.", nameof(handle));
		}

		public void Kick(ISkypeUser user)
		{
			_wrappedChat.Kick(user.Handle);
		}

		public void Kick(string handle)
		{
			_wrappedChat.Kick(handle);
		}

		/// <summary>
		/// Sends a new message to this chat.
		/// </summary>
		/// <param name="messageText">The message text.</param>
		/// <returns>An object representing the message, or null if the message is a Skype system command due to a bug in the API.</returns>
		public ISkypeMessage SendMessage(string messageText)
		{
			try
			{
				return new SkypeMessage(_wrappedChat.SendMessage(messageText));
			}
			catch (System.Runtime.InteropServices.COMException) when (messageText[0] == '/')
			{
				// Trying to send system commands to Skype throws, but everything still happens as intended otherwise??
				return null;
			}
		}

		public void AddMembers(IEnumerable<ISkypeUser> users)
		{
			var collection = new UserCollection();
			foreach (var user in users)
			{
				var userImpl = user as SkypeUser;
				if (userImpl != null)
					collection.Add(userImpl.WrappedObject);
			}

			_wrappedChat.AddMembers(collection);
			_wrappedChat.AcceptAdd();
		}

		public ISkypeUser FindUserByNameOrHandle(string searchValue)
		{
			if (searchValue == null)
				throw new ArgumentNullException(nameof(searchValue));

			foreach (ISkypeUser user in GetMembers())
			{
				if (user.Handle == searchValue
					|| user.FullName.ToUpperInvariant() == searchValue.ToUpperInvariant()
					|| (searchValue.Length > 3 && user.FullName.ToUpperInvariant().Contains(searchValue.ToUpperInvariant())))
				{
					return user;
				}
			}

			return null;
		}

		private class MessageTimestampComparer : IComparer<SkypeMessage>
		{
			public int Compare(SkypeMessage x, SkypeMessage y)
			{
				return x.Timestamp.CompareTo(y.Timestamp);
			}
		}

		public IList<ISkypeMessage> GetRecentMessages()
		{
			var result = new SkypeMessage[_wrappedChat.RecentMessages.Count];

			for (int i = _wrappedChat.RecentMessages.Count; i > 0; i--)
			{
				int index = _wrappedChat.RecentMessages.Count - i;
				result[index] = new SkypeMessage(_wrappedChat.RecentMessages[i]);
			}

			Array.Sort(result, new MessageTimestampComparer());
			return result;
		}
	}
}
