using DiceBot.Dice;
using DiceBot.Statistics;
using DiceBot.Skype.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceBot.Skype
{
	public class SkypeDiceHandler
	{
		IDieStats statsDatabase;
		IDieParser dieParser;

		public event EventHandler<UserRollMessageSentEventArgs> UserRollMessageSent;
		public bool StatsEnabled { get; set; }

		/// <summary>
		/// Initialises a new instance of the SkypeDiceHandler class.
		/// </summary>
		/// <param name="parser">The die parser to use.</param>
		/// <param name="database">A database implementation to store statistics in.</param>
		public SkypeDiceHandler(IDieParser parser, IDieStats database)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));

			if (database == null)
				throw new ArgumentNullException(nameof(database));

			statsDatabase = database;
			dieParser = parser;
		}

		/// <summary>
		/// Initialises a new instance of the SkypeDiceHandler class.
		/// </summary>
		/// <param name="parser">The die parser to use.</param>
		public SkypeDiceHandler(IDieParser parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));

			statsDatabase = null;
			dieParser = parser;
		}

		public void HandleRolls(object sender, SkypeMessageReceivedEventArgs e)
		{
			ISkypeMessage message = e.Message;

			RollGroupResult[] rolls = null;
			try
			{
				rolls = dieParser.ParseString(message.Body);
			}
			catch (Exception x)
			{
				if (x is DiceException)
					message.Chat.SendMessage($"{message.SenderFullName} is a dummy.");
				else
					throw;
			}

			if (rolls == null)
				return;

			string response = $"{message.SenderFullName} rolled:\n";

			foreach (var roll in rolls)
			{
				response += $"{roll.GroupString}: {roll}\n";

				if (StatsEnabled)
					RecordStats(message.SenderHandle, roll.GroupRolls);
			}

			message.Chat.SendMessage(response);
			UserRollMessageSent?.Invoke(this, new UserRollMessageSentEventArgs(message.SenderFullName, response));
		}
	
		private void RecordStats(string user, IEnumerable<DieRollSeries> diceRolls)
		{
			if (statsDatabase == null)
				return;

			foreach (var dieRolls in diceRolls)
			{
				if (dieRolls.Rolls.Count == 1)
					statsDatabase.RecordRoll(user, dieRolls.Die, dieRolls.Rolls.First());
				else
					statsDatabase.RecordRollBatch(user, dieRolls.Die, dieRolls.Rolls);
			}
		}
		
	}
}
