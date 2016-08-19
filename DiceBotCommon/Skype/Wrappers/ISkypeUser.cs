namespace DiceBot.Skype.Wrappers
{
	public interface ISkypeUser
	{
		string Handle { get; }
		string FullName { get; }
		bool IsContact { get; }
	}
}
