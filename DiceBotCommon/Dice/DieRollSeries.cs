using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DiceBot.Dice
{
	/// <summary>
	/// Encapusulates a series of n rolls of a single kind of die. (ex. 4d6 = a series of 4 rolls of a d6)
	/// Instances of this class are immutable.
	/// </summary>
	public class DieRollSeries
	{
		public ReadOnlyCollection<int> Rolls { get; }
		public int Modifier	{ get; }
		public int Die { get; }
		public int Sign { get; }
		public int Total { get; }
		
		private string rollString = null;
		public string RollString
		{
			get
			{
				if (rollString == null)
					rollString = string.Format("{0:+;-}{1}d{2}{3:+#;-#;#}", Sign, Rolls.Count, Die, Modifier);

				return rollString;
			}
		}

		public DieRollSeries(int die, int sign, int modifier, IEnumerable<int> rollsCollection)
		{
			if (sign != -1 && sign != 1)
				throw new ArgumentOutOfRangeException("The sign must be either -1 or 1.", nameof(sign));

			if (die < 2)
				throw new ArgumentOutOfRangeException(nameof(die));

			Rolls = Array.AsReadOnly(rollsCollection.ToArray());

			Sign = sign;
			Die = die;

			Modifier = modifier;

			Total = (Rolls.Sum() + Modifier) * Sign;
		}

		private string stringRepresentation = null;

		public override string ToString()
		{
			if (stringRepresentation == null)
			{
				var result = new StringBuilder("[");

				for (int i = 0; i < Rolls.Count; i++)
				{
					result.AppendFormat("{0}, ", Rolls[i]);
				}

				result.Remove(result.Length - 2, 2);
				result.Append("]");
				result.AppendFormat("{0:+#;-#;#}", Modifier);

				stringRepresentation = result.ToString();
			}

			return stringRepresentation;
		}
	}
}
