using System;

namespace DiceBot.Skype.Wrappers
{
	public class SkypeMessage : ISkypeMessage
	{
		SKYPE4COMLib.ChatMessage _wrappedMessage;

		public string Body => _wrappedMessage.Body;

		public ISkypeChat Chat { get; }

		public bool IsEditable => _wrappedMessage.IsEditable;

		public string SenderFullName => Sender.FullName;

		public string SenderHandle => Sender.Handle;

		public ISkypeUser Sender { get; }

		public DateTime Timestamp => _wrappedMessage.Timestamp;

		public MessageStatus Status => _wrappedMessage.Status.ToWrappedType();
		public MessageType Type => _wrappedMessage.Type.ToWrappedType();

		public bool Seen
		{
			set { _wrappedMessage.Seen = value; }
		}

		public SkypeMessage(SKYPE4COMLib.ChatMessage wrappedMessageObject)
		{
			if (wrappedMessageObject == null)
				throw new ArgumentNullException(nameof(wrappedMessageObject));

			_wrappedMessage = wrappedMessageObject;
			Sender = new SkypeUser(_wrappedMessage.Sender);
			Chat = new SkypeChat(_wrappedMessage.Chat);
		}

		public void Edit(string newValue)
		{
			if (!IsEditable)
				throw new InvalidOperationException("Message is not editable.");

			_wrappedMessage.Body = newValue;
		}
	}
}
