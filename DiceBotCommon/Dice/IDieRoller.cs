using DiceBot.RandomNumberGenerators;
using System;

namespace DiceBot.Dice
{
	public interface IDieRoller
	{
		event EventHandler RandomNumberGeneratorChanged;

		IRandomNumberGenerator RandomNumberGenerator { get; set; }
		int Roll(int sides);
	}
}
