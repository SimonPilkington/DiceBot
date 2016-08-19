using System;

namespace DiceBot.Skype
{
	[Serializable]
	public class SkypeException : System.Exception
	{
		public SkypeException() : base() { }
		public SkypeException(string message) : base(message) { }
		public SkypeException(string message, System.Exception inner) : base(message, inner) { }

		protected SkypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}