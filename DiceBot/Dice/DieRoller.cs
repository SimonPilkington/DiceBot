using System;
using System.Text.RegularExpressions;
using DiceBot.RandomNumberGenerators;

namespace DiceBot.Dice
{
	public class DieRoller : IDieParser, IDieRoller
	{
		private const RegexOptions options = RegexOptions.Compiled | RegexOptions.CultureInvariant;

		private static readonly Regex modifier = new Regex(@"[\+-][1-9]\d*", options);
		private static readonly Regex die = new Regex(@"([\+-])?([1-9]\d*)[dD]([1-9]\d*)((?:[\+-]\d+(?!\d*[dD]))*)", options);
		private static readonly Regex rollGroup = new Regex(@"[1-9]\d*[dD][1-9]\d*(?:[\+-](?:(?:[1-9]\d*[dD][1-9]\d*)|(?:\d+)))*", options);
		private static readonly Regex urlFilter = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", options | RegexOptions.IgnoreCase);

		public event EventHandler RandomNumberGeneratorChanged;

		IRandomNumberGenerator randomNumberGenerator;
		public IRandomNumberGenerator RandomNumberGenerator
		{
			get { return randomNumberGenerator; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				if (value != randomNumberGenerator)
				{
					randomNumberGenerator = value;
					RandomNumberGeneratorChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public DieRoller(IRandomNumberGenerator generator)
		{
			if (generator == null)
				throw new ArgumentNullException(nameof(generator));

			randomNumberGenerator = generator;
		}

		public int Roll(int sides)
		{
			if (sides < 2)
				throw new ArgumentOutOfRangeException(nameof(sides));

			return randomNumberGenerator.Sample(1, sides + 1);
		}

		private DieRollSeries[] ParseRollGroup(string rollGroup)
		{
			MatchCollection dice = die.Matches(rollGroup);
			
			if (dice.Count == 0)
				throw new ArgumentException("String does not represent a roll: " + rollGroup, nameof(rollGroup));

			var result = new DieRollSeries[dice.Count];
			int i = 0;

			foreach (Match die in dice)
			{
				DieRollSeries rolls = ParseDie(die);
				result[i++] = rolls;
			}

			return result;
		}

		private DieRollSeries ParseDie(Match die)
		{
			// Group 1 is the sign, if applicable
			// Group 2 is the number of dice
			// Group 3 is the number of sides
			// Group 4 is the modifier(s), if applicable
			
			string numDiceStr = die.Groups[2].Value;
			string dieSidesStr = die.Groups[3].Value;

			int numDice = int.Parse(numDiceStr);
			int dieSides = int.Parse(dieSidesStr);

			if (numDice > 1000 || dieSides > 100 || dieSides < 2)
				throw new DiceException("die sides or number of dice");

			var results = new int[numDice];

			for (int i = 0; i < numDice; i++)
					results[i] = Roll(dieSides);

			int modifier = string.IsNullOrEmpty(die.Groups[4].Value) ? 0 : ParseModifiers(die.Groups[4].Value);
			int sign = (string.IsNullOrEmpty(die.Groups[1].Value) || die.Groups[1].Value[0] == '+') ? 1 : -1;

			return new DieRollSeries(dieSides, sign, modifier, results);
		}

		private static int ParseModifiers(string modifierStr)
		{
			int total = 0;
			MatchCollection modifiers = modifier.Matches(modifierStr);

			foreach (Match modifier in modifiers)
			{
				string val = modifier.Value;
				total += int.Parse(val);
			}

			return total;
		}

		public RollGroupResult[] ParseString(string value)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			value = urlFilter.Replace(value, ""); // get rid of URLs since they produce frivolous rolls

			var dieStringsCount = die.Matches(value).Count; // the number of substrings representing die rolls found in the string (NOT the number of dice to roll)

			if (dieStringsCount > 20)
				throw new DiceException("Too many dice.");

			if (dieStringsCount == 0)
				return null;

			MatchCollection rollGroups = rollGroup.Matches(value); // There should be at least one roll group if diceCount > 0
			var rollGroupResults = new RollGroupResult[rollGroups.Count];

			for (int i = 0; i < rollGroups.Count; i++)
			{
				DieRollSeries[] dieRolls = ParseRollGroup(rollGroups[i].Value);
				rollGroupResults[i] = new RollGroupResult(rollGroups[i].Value, dieRolls);
			}

			return rollGroupResults;
		}
	}
}
