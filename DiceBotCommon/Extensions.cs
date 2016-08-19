using System;

namespace DiceBot
{
	public static class Extensions
	{
		private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long ToUnixTime(this DateTime value)
		{
			var instant = value.ToUniversalTime() - epoch;

			long unixTime = instant.Ticks / 10000000L;
			return unixTime;
		}

		public static DateTime DateTimeFromUnixTime(ulong unixTime)
		{
			return epoch.AddSeconds(unixTime);
		}
	}
}
