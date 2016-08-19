namespace DiceBot.Skype.Wrappers
{
	public interface ISkypeMessage
	{
		bool IsEditable { get; }
		string Body { get; }
		void Edit(string newValue);

		string SenderHandle { get; }
		string SenderFullName { get; }

		ISkypeChat Chat { get; }
		ISkypeUser Sender { get; }

		MessageStatus Status { get; }
		MessageType Type { get; }

		System.DateTime Timestamp { get; }

		bool Seen { set; }
	}
}
