using System.Collections.Generic;

namespace DiceBot.Statistics
{
	public interface IDieStats
	{
		void RecordRoll(string userHandle, int die, int value);
		void RecordRollBatch(string userHandle, int die, IEnumerable<int> values);
	}
}
