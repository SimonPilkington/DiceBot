using System;
using DiceBot.RandomNumberGenerators;
using System.Threading.Tasks;
using System.Threading;
using DiceBot.Dice;
using DiceBot.Statistics;
using DiceBot.Skype;
using DiceBot.Plugins;
using System.Collections.Generic;
using DiceBot.Skype.Wrappers;

namespace DiceBot.Ui
{
	public sealed class MainPresenter : IDisposable
	{
		private DieRoller roller;
		private StatsDatabase statsDatabase;

		private ISkypeController skypeController;

		private SkypeBotCommandHandler commandHandler;
		private SkypeDiceHandler diceHandler;
		
		private IMainView view;
		private IEnumerable<IDiceBotPlugin> plugins = System.Linq.Enumerable.Empty<IDiceBotPlugin>();

		public MainPresenter(IMainView view)
		{
			this.view = view;
			InitStats();

#if DEBUG
			roller = new DieRoller(new UniformRandomNumberGenerator());
			view.ConsolePrint("DEBUG BUILD - Skype functions unavailable.\n");
			view.InteractionEnabled = true;
			view.OperatorHandleTextChanged += (s, e) => Properties.Settings.Default.OperatorHandle = view.OperatorHandle;
#else
			skypeController = new SkypeController();
			skypeController.SkypeUserMessageReceived += (s, e) => DebugPrint($"Received message ({e.Message.Type}): {e.Message.Body}\n");
			skypeController.SkypeSystemMessageReceived += (s, e) => DebugPrint($"Received message ({e.Message.Type}): {e.Message.Body}\n");
			skypeController.ContactRequestAccepted += (s, e) => view.ConsolePrint($"Added {e.UserName}.\n");
			
			view.OperatorHandleTextChanged += (s, e) =>	skypeController.OperatorHandle = view.OperatorHandle;

			InitDice();
			InitCommands();
			LoadPlugins();

			// Do all initial work synchronously, then attach last so we don't process any messages before everything is fully hooked up.
			var attachTask = AttachToSkype();

			var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext(); // Come back to the UI thread for the continuation.

			attachTask.ContinueWith((task) =>
			{
				if (task.IsFaulted)
				{
					if (task.Exception.InnerException is SkypeException)
						view.ConsolePrint($"Failed: {task.Exception.InnerException.Message}\n");
					else
						view.ConsolePrint($"Failed. An unexpected error occurred:\n\n{task.Exception.InnerException}\n\n{task.Exception.InnerException.StackTrace}");
				}
				else
				{
					view.ConsolePrint("Ready.\n");
					view.InteractionEnabled = true;
				}
			}, CancellationToken.None, TaskContinuationOptions.None, uiScheduler);
#endif
		}

		private void ShowStats()
		{
			StatsWindow.ShowStatsWindow(statsDatabase);
		}

		private void InitStats()
		{
			view.ConsolePrint("Initialising stats database... ");

			statsDatabase = new StatsDatabase();
			view.StatsButtonClick += (s, e) => ShowStats();

			view.ConsolePrint("OK.\n", false);
		}

		private Task AttachToSkype()
		{
			skypeController.AttachedStateChanged += (s, e) =>
			{
				if (skypeController.State == AttachedState.AuthorizationPending)
					view.ConsolePrint("Waiting for authorisation.\n");

				else if (skypeController.State == AttachedState.Attached)
					view.ConsolePrint("Authorised.\n");
			};

			view.ConsolePrint("Attaching to Skype... \n");

			// This could block until Skype gives us authorisation, so we'll be moving off the UI thread.
			return Task.Factory.StartNew(() =>
			{
				skypeController.Attach();
				skypeController.OperatorHandle = view.OperatorHandle;
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
		}

		private void LoadPlugins()
		{
			Action<System.Windows.Forms.TabPage> addTab = (tab) => view.AddPluginTab(tab);
			Action<System.Windows.Forms.TabPage> removeTab = (tab) => view.RemovePluginTab(tab);
			Action<string, Action<ISkypeMessage>> addCommand = (cmd, act) => commandHandler.AddCommand(cmd, act);

			plugins = PluginLoader.Instance.LoadPlugins();

			foreach (IDiceBotPlugin plugin in plugins)
			{
				// No try/catch here. If plugin init throws, we can't assume it didn't corrupt our state.
				Action<string> notify = (s) => view.ConsolePrint($"{plugin.Id}: {s}", true);

				plugin.Init(notify, addCommand, addTab, removeTab, roller, skypeController);
				view.KeyPress += plugin.KeyPressedEventHandler;
				view.ConsolePrint($"Loaded plugin '{plugin.Id} v{plugin.Version}'.\n");
			}
		}

		private void InitCommands()
		{
			view.ConsolePrint("Initialising chat commands... ");

			commandHandler = new SkypeBotCommandHandler(skypeController, statsDatabase);
			skypeController.SkypeUserMessageReceived += commandHandler.HandleBotCommand;

			commandHandler.ApocalypseInitiated += (s, e) => view.ConsolePrint($"Apocalypse initiated in {e.ChatName} by {e.Initiator}.\n");
			commandHandler.GroupCreated += (s, e) => view.ConsolePrint($"Created group for {e.Requester}.\n");
			commandHandler.SlapAttempted += (s, e) =>
			{
				string message = string.Empty;
				if (e.Result == SkypeBotCommandHandler.SlapResult.Hit)
					message += $"{e.UserName} slapped {e.TargetName}";
				else if (e.Result == SkypeBotCommandHandler.SlapResult.HitSelf)
					message += $"{e.UserName} slapped themselves";
				else
					message += $"{e.UserName} missed";

				view.ConsolePrint(message + $" in {e.ChatName}.\n");
			};

			view.ConsolePrint("OK.\n", false);
		}

		private void InitDice()
		{
			view.ConsolePrint("Initialising dice... ");

			roller = new DieRoller(new UniformRandomNumberGenerator());
			diceHandler = new SkypeDiceHandler(roller, statsDatabase);

			diceHandler.StatsEnabled = view.StatsEnabled;
			diceHandler.UserRollMessageSent += (s, e) => view.ConsolePrint($"{e.Message}");

			skypeController.SkypeUserMessageReceived += diceHandler.HandleRolls;
			
			view.StatsEnabledChanged += (s, e) => diceHandler.StatsEnabled = view.StatsEnabled;
			view.ConsolePrint("OK.\n", false);
		}
		
		private void DebugPrint(string text, bool timeStamp = true)
		{
			if (view.DebugEnabled)
				view.ConsolePrint(text, timeStamp);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				statsDatabase?.Dispose();

				foreach (var plugin in plugins)
					plugin.Dispose();
			}
		}
	}
}
