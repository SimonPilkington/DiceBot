namespace DiceBot.RandomNumberGenerators
{
	public static class RandomProvider
	{
		private static readonly RandomWithNotify generator = new RandomWithNotify();

		public static RandomWithNotify Instance => generator;
	}
}
