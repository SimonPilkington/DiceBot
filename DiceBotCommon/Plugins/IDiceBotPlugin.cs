using DiceBot.Dice;
using DiceBot.Skype.Wrappers;
using System;
using System.Windows.Forms;
using System.ComponentModel.Composition;

namespace DiceBot.Plugins
{
	/// <summary>
	/// Provides an interface for initialising plugins. Plugins are loaded using MEF in the main application, and the interface is given the InheritedExport attribute here so that plugins need not be aware of MEF.
	/// </summary>
	[InheritedExport(typeof(IDiceBotPlugin))]
	public interface IDiceBotPlugin : IDisposable
	{
		// Plugins will receive actions allowing them to post string notifications to the main UI, add chat commands, add/remove UI tabs (tabs should be 510x151), 
		// as well as access to the current die roller state, and pseudo-random number generator state, and the Skype controller.
		void Init(Action<string> notify, Action<string, Action<ISkypeMessage>> addCommand, Action<TabPage> addTab, Action<TabPage> removeTab, IDieRoller dieRoller, Skype.ISkypeController controller);

		// Each plugin must provide an identifier.
		string Id { get; }
		Version Version { get; }

		// Plugins can receive keyboard input from the main UI.
		void KeyPressedEventHandler(object sender, KeyPressEventArgs e);
	}
}
