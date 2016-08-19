namespace DiceBot.Dice
{
	public interface IDieParser
	{
		RollGroupResult[] ParseString(string value);
	}
}
