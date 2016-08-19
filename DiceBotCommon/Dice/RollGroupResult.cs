using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DiceBot.Dice
{
	/// <summary>
	/// Encapsulates the result of a single roll group. (ex. 2d10+5+4d6-2d4)
	/// Instances of this class are immutable.
	/// </summary>
	public class RollGroupResult
	{
		public ReadOnlyCollection<DieRollSeries> GroupRolls { get; }	
		public int Total { get; }
		public string GroupString { get; }

		public RollGroupResult(string groupString, IEnumerable<DieRollSeries> series)
		{
			GroupString = groupString;
			GroupRolls = Array.AsReadOnly(series.ToArray());
			Total = GroupRolls.Sum((e) => e.Total);
		}

		private string stringRepresentation = null;

		public override string ToString()
		{
			if (stringRepresentation == null)
			{
				var result = new StringBuilder(128);

				bool needSign = false;
				foreach (var dieRolls in GroupRolls)
				{
					if (needSign)
						result.AppendFormat("{0:+;-;}", dieRolls.Sign);

					needSign = true;
					result.Append(dieRolls);
				}

				result.AppendFormat(" = {0}", Total);
				stringRepresentation = result.ToString();
			}

			return stringRepresentation;
		}
	}
}
