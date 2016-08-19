using DiceBot.Statistics;
using DiceBot.Skype.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

namespace DiceBot.Skype
{
	public class SkypeBotCommandHandler
	{
		private ISkypeController skypeController;
		private IReadDieStats database;

		private Dictionary<string, Action<ISkypeMessage>> CommandMapping;

		public SkypeBotCommandHandler(ISkypeController controller, IReadDieStats statsDatabase)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			skypeController = controller;
			database = statsDatabase;

			const string helpMessage = "XdY to roll X Y-sided dice.\n!newgroup to create groups.\n!stats to check die statistics. (Dialogue Only)\nSecret commands for being dumb.";
			Action<ISkypeMessage> HelpCommand = (message) => message.Chat.SendMessage(helpMessage);

			CommandMapping = new Dictionary<string, Action<ISkypeMessage>>()
				{
					{"!newgroup", CreateGroup},
					{"!slap", Slap},
					{"!superslap", SuperSlap },
					{"!stats", ShowStats },
					{"!apocalypse", Apocalpse },
					{"!diediedie", Apocalpse },
					{"!thirdimpact", Apocalpse },
					{"!help",  HelpCommand },
					{"!about", HelpCommand },
					{"!incdate", IncrementDate },
					{"!bye", Bye }
				};
		}

		public void HandleBotCommand(object sender, SkypeMessageReceivedEventArgs e)
		{
			var message = e.Message;

			if (message.Body.Length == 0 || message.Body[0] != '!')
				return;

			char[] spaceSeparator = new[] { ' ' };
			string command = message.Body.Split(spaceSeparator, 2)[0];

			Action<ISkypeMessage> action;
			if (CommandMapping.TryGetValue(command, out action))
				action(message);
		}

		public void AddCommand(string command, Action<ISkypeMessage> action)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			if (action == null)
				throw new ArgumentNullException(nameof(action));

			if (CommandMapping.ContainsKey($"!{command}"))
				throw new ArgumentException("Command already exists.", nameof(command));

			CommandMapping.Add(command, action);
		}

		private static readonly Regex shortDateRegex = new Regex(@"\b\d{1,2}[\./]\d{1,2}[\./]\d{1,4}\b", RegexOptions.Compiled);
		private void IncrementDate(ISkypeMessage message)
		{
			if (message.Chat.Type != ChatType.Group)
				return;

			DateTime? currDate = null;
			foreach (Match m in shortDateRegex.Matches(message.Chat.Topic))
			{
				DateTime dt;
				if (DateTime.TryParse(m.Value, out dt))
				{
					currDate = dt;
					break;
				}
			}

			if (currDate != null)
			{
				var parms = ParseMessageParams(message, typeof(uint));
				uint days = parms[0] == null ? 1 : (uint)parms[0];

				DateTime newDate = currDate.Value.AddDays(days);
				message.Chat.Topic = shortDateRegex.Replace(message.Chat.Topic, newDate.ToString("dd/MM/yyyy"));
			}
		}

		private void CreateGroup(ISkypeMessage message)
		{
			if (!message.Sender.IsContact)
			{
				message.Chat.SendMessage(message.SenderFullName + " needs to be on my contact list.");
				return;
			}			
			
			ISkypeChat newGroup = skypeController.CreateGroupChat(message.Sender);
			newGroup.SendMessage("Group created.");
			newGroup.SendMessage("/setrole " + message.SenderHandle + " MASTER");

			GroupCreated?.Invoke(this, new GroupCreatedEventArgs(message.SenderFullName));
		}

		private void Slap(ISkypeMessage message)
		{
			ISkypeUser user = message.Sender;

			string targetParam = ParseWholeStringParam(message);

			if (targetParam == null)
				return;

			ISkypeUser target = message.Chat.FindUserByNameOrHandle(targetParam);

			if (target == null)
			{
				OnSlapAttempted(message.SenderFullName, string.Empty, message.Chat.Name, SlapResult.Missed);
				return;
			}

			if (target.Handle != skypeController.BotHandle)
			{
				message.Chat.SendMessage(user.FullName + " slaps " + target.FullName + " around a bit with a large trout.");
				OnSlapAttempted(message.SenderFullName, target.FullName, message.Chat.Name, SlapResult.Hit);
			}
			else if (message.Chat.MyRole == ChatRole.Creator && target.Handle == skypeController.BotHandle)
			{
				message.Chat.SendMessage(user.FullName + " slaps " + target.FullName + " around a bit with a large trout.");
				message.Chat.Kick(user.Handle);
				message.Chat.SendMessage("And regrets it immediately.");

				OnSlapAttempted(message.SenderFullName, string.Empty, message.Chat.Name, SlapResult.HitSelf);
			}
		}

		private void SuperSlap(ISkypeMessage message)
		{
			var user = message.Sender;

			if (message.Chat.MyRole != ChatRole.Creator)
				return;

			string targetParam = ParseWholeStringParam(message);
			if (targetParam == null)
			{
				OnSlapAttempted(message.SenderFullName, string.Empty, message.Chat.Name, SlapResult.Missed);
				return;
			}

			var target = message.Chat.FindUserByNameOrHandle(targetParam);

			if (target == null || target.Handle == skypeController.BotHandle)
			{
				message.Chat.SendMessage($"{user.FullName} misses.");
				message.Chat.Kick(user.Handle);

				OnSlapAttempted(message.SenderFullName, string.Empty, message.Chat.Name, SlapResult.HitSelf);
				return;
			}

			message.Chat.Kick(target.Handle);
			OnSlapAttempted(message.SenderFullName, target.FullName, message.Chat.Name, SlapResult.Hit);
		}

		private HashSet<Timer> activeTimers = new HashSet<Timer>(); // To prevent garbage collection.

		private void Apocalpse(ISkypeMessage message)
		{
			const int APOCALYPSE_LENGTH_MILISECONDS = 7000;

			if (message.Chat.Type == ChatType.Dialog || message.Chat.MyRole != ChatRole.Creator)
				return;

			ApocalypseInitiated?.Invoke(this, new ApocalypseInitiatedEventArgs(message.SenderFullName, message.Chat.Name));
			var collection = new List<ISkypeUser>();

			
			foreach (var user in message.Chat.GetMembers().ToList())
			{
				if (user.Handle == skypeController.BotHandle || !user.IsContact)
					continue;

				collection.Add(user);
				message.Chat.Kick(user.Handle);
			}

			Timer timer = null;
			timer = new Timer((c) => { message.Chat.AddMembers((List<ISkypeUser>)c); activeTimers.Remove(timer); }, 
				collection, Timeout.Infinite, Timeout.Infinite);

			activeTimers.Add(timer);
			timer.Change(APOCALYPSE_LENGTH_MILISECONDS, Timeout.Infinite);
		}

		private void ShowStats(ISkypeMessage message)
		{
			var chat = message.Chat;
			if (chat.Type != ChatType.Dialog)
			{
				chat.SendMessage("!stats is too spammy to be used in groups. Message the bot directly.");
				return;
			}

			if (database == null)
			{
				chat.SendMessage("Not available.");
				return;
			}

			long? uid = database.GetUid(message.SenderHandle);
			if (uid == null)
			{
				chat.SendMessage("No stats found for " + message.SenderFullName + ".");
				return;
			}

			var statsString = new StringBuilder("Statistics for " + message.SenderFullName);

			var parms = ParseMessageParams(message, typeof(DateTime));

			DateTime sinceDate = parms[0] == null ? DateTime.MinValue : (DateTime)parms[0];
			sinceDate = DateTime.SpecifyKind(sinceDate, DateTimeKind.Utc);

			if (sinceDate > DateTime.MinValue)
				statsString.AppendFormat(" since midnight {0:dd/MM/yyy} (UTC):\n\n", sinceDate);
			else
				statsString.Append(":\n\n");

			bool none = true;
			foreach (var row in database.GetStatsForUid(uid.Value, sinceDate))
			{
				none = false;
				statsString.AppendFormat(
					"d{0}:\n" +
					"Mean: {1:0.####}\n" +
					"Standard deviation: {2:0.####}\n" +
					"Natural ones: {3}\n\n",
					row.Die, row.Mean, row.StdDev, row.NaturalOnes);
			}

			if (none)
				statsString.Append("None.");

			chat.SendMessage(statsString.ToString());
		}

		private void Bye(ISkypeMessage message)
		{
			IList<ISkypeMessage> recent = message.Chat.GetRecentMessages();

			if (message.Chat.MyRole != ChatRole.Creator || recent.Count <= 1)
				return;

			ISkypeMessage relevantMessage = null;

			for (int i = 0; i < recent.Count; i++)
			{
				if (recent[i].Type == MessageType.Said)
				{
					relevantMessage = recent[i];
					break;
				}
			}

			if (relevantMessage != null)
				message.Chat.Kick(relevantMessage.Sender);
		}

		private static object[] ParseMessageParams(ISkypeMessage message, params Type[] types)
		{
			object[] result = new object[types.Length];
			string[] paramsStrings = message.Body.Split(new[] { ' ' }, types.Length + 2);

			// Length + 2 because otherwise the last param could end up in an invalid form
			// ex for !incdate 4 43 it would yield "4 43", which is invalid, when "4" is a valid param

			for (int i = 0; i < types.Length && i < paramsStrings.Length; i++)
			{
				if (types[i] == null)
					continue;

				try
				{
					result[i] = Convert.ChangeType(paramsStrings[i + 1], types[i]);
				}
				catch (Exception e) when (e is IndexOutOfRangeException || e is FormatException 
											|| e is OverflowException || e is InvalidCastException)
				{ } 
				
				// If we couldn't convert, swallow the exception and return null for that param. The caller should handle it.
			}

			return result;
		}

		private static string ParseWholeStringParam(ISkypeMessage message)
		{
			string[] paramsStrings = message.Body.Split(new[] { ' ' }, 2);

			if (paramsStrings.Length < 2)
				return null;

			return paramsStrings[1];
		}

		#region Events
		public event EventHandler<GroupCreatedEventArgs> GroupCreated;

		public enum SlapResult { Missed, HitSelf, Hit };
		public event EventHandler<SlapAttemptedEventArgs> SlapAttempted;

		public void OnSlapAttempted(string user, string target, string chatName, SlapResult result)
		{
			SlapAttempted?.Invoke(this, new SlapAttemptedEventArgs(user, target, chatName, result));
		}

		public event EventHandler<ApocalypseInitiatedEventArgs> ApocalypseInitiated;
		#endregion
	}
}
