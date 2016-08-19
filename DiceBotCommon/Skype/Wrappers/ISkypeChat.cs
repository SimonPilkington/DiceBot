using System.Collections.Generic;

namespace DiceBot.Skype.Wrappers
{
	public interface ISkypeChat
	{
		string Topic { get; set; }
		string Name { get; }
		string Guid { get; }

		ChatType Type { get; }

		void Kick(string handle);
		void Kick(ISkypeUser user);

		ChatRole GetRole(string handle);
		ChatRole GetRole(ISkypeUser user);
		ChatRole MyRole { get; }

		ISkypeMessage SendMessage(string messageText);

		IEnumerable<ISkypeUser> GetMembers();
		void AddMembers(IEnumerable<ISkypeUser> users);

		IList<ISkypeMessage> GetRecentMessages();

		ISkypeUser FindUserByNameOrHandle(string searchValue);
	}
}
