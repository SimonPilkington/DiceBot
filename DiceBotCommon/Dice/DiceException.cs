using System;

namespace DiceBot.Dice
{
	[Serializable]
	public class DiceException : System.Exception
	{
		public DiceException() : base() { }
		public DiceException(string message) : base(message) { }
		public DiceException(string message, System.Exception inner) : base(message, inner) { }

		protected DiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}