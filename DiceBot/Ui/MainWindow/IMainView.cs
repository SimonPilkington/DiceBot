using System;
using System.Windows.Forms;

namespace DiceBot.Ui
{
	public interface IMainView
	{
		bool DebugEnabled { get; }
		bool StatsEnabled { get; }

		bool InteractionEnabled { get; set; }

		string OperatorHandle { get; }

		/// <summary>
		/// Print to the main UI console. The implementation should be safe to call from non-UI threads.
		/// </summary>
		/// <param name="text">The text to print.</param>
		/// <param name="timestamp">Whether to print a leading timestamp.</param>
		void ConsolePrint(string text, bool timestamp = true);
		void AddPluginTab(TabPage tab);
		void RemovePluginTab(TabPage tab);

		event EventHandler StatsButtonClick;
		event KeyPressEventHandler KeyPress; // Must set KeyPreview to true in view.
		event EventHandler StatsEnabledChanged;
		event EventHandler OperatorHandleTextChanged;
	}
}
