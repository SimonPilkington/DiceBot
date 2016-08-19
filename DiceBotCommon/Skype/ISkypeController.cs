using DiceBot.Skype.Wrappers;
using System;
using System.Collections.Generic;

namespace DiceBot.Skype
{
	public interface ISkypeController
	{
		event EventHandler<SkypeMessageReceivedEventArgs> SkypeUserMessageReceived;
		event EventHandler<SkypeMessageReceivedEventArgs> SkypeSystemMessageReceived;
		event EventHandler<ContactRequestAcceptedEventArgs> ContactRequestAccepted;

		AttachedState State { get; }
		event EventHandler AttachedStateChanged;
		void Attach();

		string BotHandle { get; }
		string OperatorHandle { get; set; }
		
		void MessageOperator(string text);

		ISkypeChat CreateGroupChat();
		ISkypeChat CreateGroupChat(IEnumerable<ISkypeUser> users);
		ISkypeChat CreateGroupChat(ISkypeUser user);
	}
}
