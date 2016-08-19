using System;

namespace DiceBot.RandomNumberGenerators
{
	public class UniformRandomNumberGenerator : IRandomNumberGenerator
	{
		public int Sample(int lowerBound, int upperBound)
		{
			return RandomProvider.Instance.Next(lowerBound, upperBound);
		}

		public int Sample(Random rng, int lowerBound, int upperBound)
		{
			return rng.Next(lowerBound, upperBound);
		}

		public int Sample(int upperBound)
		{
			return Sample(1, upperBound);
		}

		public int Sample(Random rng, int upperBound)
		{
			return Sample(rng, 1, upperBound);
		}
	}
}
