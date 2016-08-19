using System;

namespace DiceBot.RandomNumberGenerators
{
	public interface IRandomNumberGenerator
	{
		int Sample(int lowerBound, int upperBound);
		int Sample(Random rng, int lowerBound, int upperBound);
		int Sample(int upperBound); // lower bound implicitly 1
		int Sample(Random rng, int upperBound); // lower bound implicitly 1
	}
}
