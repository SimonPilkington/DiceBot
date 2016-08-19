namespace DiceBot.Statistics
{
	public struct StatsRow
	{
		public StatsRow(int die, double mean, double stdDev, int natOnes)
		{
			Die = die; Mean = mean; StdDev = stdDev; NaturalOnes = natOnes;
		}

		public int Die
		{ get; private set; }

		public double Mean
		{ get; private set; }
		public double StdDev
		{ get; private set; }
		public int NaturalOnes
		{ get; private set; }
	}
}
