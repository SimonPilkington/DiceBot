using DiceBot.Skype.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DiceBot.Skype
{
	public class SkypeController : ISkypeController
	{
		public string OperatorHandle { get;  set; }
		
		public event EventHandler<SkypeMessageReceivedEventArgs> SkypeUserMessageReceived;
		public event EventHandler<SkypeMessageReceivedEventArgs> SkypeSystemMessageReceived;
		public event EventHandler<ContactRequestAcceptedEventArgs> ContactRequestAccepted;

		public event EventHandler AttachedStateChanged;

		private SKYPE4COMLib.Skype skypeConnection = new SKYPE4COMLib.Skype();
		private AttachedState _state;
		public AttachedState State
		{
			get { return _state; }
			private set
			{
				if (_state != value)
				{
					if (_state > value)
						throw new InvalidOperationException("State cannot not move backwards."); // either we attach successfully, or we throw

					_state = value;
					AttachedStateChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public string BotHandle
		{
			get
			{
				CheckAttached();
				return skypeConnection.CurrentUserHandle;
			}
		}

		private void CheckAttached()
		{
			if (_state != AttachedState.Attached)
				throw new InvalidOperationException("Not attached to Skype.");
		}
		
		public SkypeController()
		{
			_state = AttachedState.Detached;
		}

		public void Attach()
		{
			skypeConnection.Attach(7, false);

			var skypeInterface = (SKYPE4COMLib.ISkype)skypeConnection;
			if (skypeInterface.AttachmentStatus.HasFlag(SKYPE4COMLib.TAttachmentStatus.apiAttachRefused))
				throw new SkypeException("Authorisation refused or Skype not running.");

			State = AttachedState.AuthorizationPending;
			while (skypeInterface.AttachmentStatus.HasFlag(SKYPE4COMLib.TAttachmentStatus.apiAttachPendingAuthorization))
			{
				const int AUTH_RECHECK_INTERVAL_MILISECONDS = 500;

				if (skypeInterface.AttachmentStatus.HasFlag(SKYPE4COMLib.TAttachmentStatus.apiAttachRefused))
					throw new SkypeException("Authorisation refused.");

				Thread.Sleep(AUTH_RECHECK_INTERVAL_MILISECONDS);
			}

			State = AttachedState.Attached;

			skypeConnection.MessageStatus += MainMessageHandler;
			skypeConnection.UserAuthorizationRequestReceived += ContactRequest;
		}

		private void MainMessageHandler(SKYPE4COMLib.ChatMessage message, SKYPE4COMLib.TChatMessageStatus status)
		{
			if (status != SKYPE4COMLib.TChatMessageStatus.cmsReceived) // We don't care about sent messages and the sort.
				return;

			// Ignore messages older than a minute. (In case we were offline for whatever reason. Users likely don't care about commands they issued yesterday.)
			if (message.Timestamp < DateTime.Now.AddMinutes(-1))
			{
				message.Seen = true;
				return;
			}

			var wrappedMessage = new SkypeMessage(message);
			
			if (message.Type == SKYPE4COMLib.TChatMessageType.cmeSaid && SkypeUserMessageReceived != null)
				InvokeMessageEventHandler(SkypeUserMessageReceived, wrappedMessage);
			else if (SkypeSystemMessageReceived != null)
				InvokeMessageEventHandler(SkypeSystemMessageReceived, wrappedMessage);
		}

		private void ContactRequest(SKYPE4COMLib.User contact)
		{
			const string cloudChatWarning = "NB: If I'm added to a group but don't respond to commands, the group is cloud-based. " +
			"There's nothing you can do about this except create a new, peer-to-peer-based group by messaging me with \"!newgroup\".";

			if (contact.IsAuthorized)
				return;

			// Auto-accept all contact requests.
			contact.IsAuthorized = true;
			ContactRequestAccepted?.Invoke(this, new ContactRequestAcceptedEventArgs(contact.FullName));
			
			skypeConnection.SendMessage(contact.Handle, cloudChatWarning);
		}

		public void MessageOperator(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			CheckAttached();

			if (string.IsNullOrWhiteSpace(OperatorHandle))
				return;

			skypeConnection.SendMessage(OperatorHandle, text);
		}

		public ISkypeChat CreateGroupChat()
		{
			CheckAttached();
			return new SkypeChat(skypeConnection.CreateChatWith(""));
		}

		public ISkypeChat CreateGroupChat(IEnumerable<ISkypeUser> users)
		{
			if (users == null)
				throw new ArgumentNullException(nameof(users));

			// Must use this because a dialogue is created instead of a group chat when only one user is in the collection.
			var chat = CreateGroupChat();
			chat.AddMembers(users);

			return chat;
		}

		public ISkypeChat CreateGroupChat(ISkypeUser user)
		{
			if (user == null)
				throw new ArgumentNullException(nameof(user));

			return CreateGroupChat(new[] { user });
		}

		private void InvokeMessageEventHandler(EventHandler<SkypeMessageReceivedEventArgs> handler, SkypeMessage message)
		{
			var eventArgs = new SkypeMessageReceivedEventArgs(message);
			handler(this, eventArgs);

			if (!eventArgs.CancelSetSeen)
				message.Seen = true;
		}
	}
}
