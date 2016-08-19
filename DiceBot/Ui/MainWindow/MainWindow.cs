using System;
using System.Windows.Forms;

namespace DiceBot.Ui
{
	public partial class MainWindow : Form, IMainView
	{
		public event EventHandler StatsButtonClick
		{
			add { statsButton.Click += value; }
			remove { statsButton.Click -= value; }
		}

		public event EventHandler StatsEnabledChanged
		{
			add { statsEnabledCheckBox.CheckedChanged += value; }
			remove { statsEnabledCheckBox.CheckedChanged -= value; }
		}

		public event EventHandler OperatorHandleTextChanged
		{
			add { operatorTextBox.TextChanged += value; }
			remove { operatorTextBox.TextChanged -= value; }
		}

		public bool DebugEnabled => debugCheckbox.Checked;

		public bool StatsEnabled => statsEnabledCheckBox.Checked;

		public bool InteractionEnabled
		{
			get { return mainPanel.Enabled; }
			set { mainPanel.Enabled = value; }
		}

		public string OperatorHandle => operatorTextBox.Text;

		public MainWindow()
		{
			InitializeComponent();

			Text = Text + $" {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

			KeyPreview = true;
			//CheckForIllegalCrossThreadCalls = true;
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			mainConsole.Clear();
		}

		public void ConsolePrint(string text, bool timestamp)
		{
			if (mainConsole.InvokeRequired)
			{
				mainConsole.Invoke(new Action(() => ConsolePrint(text, timestamp)));
				return;
			}

			if (timestamp)
				mainConsole.AppendText($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")}: ");

			text = text.Replace("\n", Environment.NewLine);
			mainConsole.AppendText(text);
		}

		public void AddPluginTab(TabPage tab)
		{
			pluginTabControl.TabPages.Add(tab);
		}

		public void RemovePluginTab(TabPage tab)
		{
			pluginTabControl.TabPages.Remove(tab);
		}
		
		private void MainWindow_Load_SetupProperties(object sender, EventArgs e)
		{
			operatorTextBox.Text = Properties.Settings.Default.OperatorHandle;
			operatorTextBox.TextChanged += (s, ea) => Properties.Settings.Default.OperatorHandle = operatorTextBox.Text;

			statsEnabledCheckBox.Checked = Properties.Settings.Default.RecordStats;
			statsEnabledCheckBox.CheckedChanged += (s, ea) => Properties.Settings.Default.RecordStats = statsEnabledCheckBox.Checked;
		}

		private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			Properties.Settings.Default.Save();
		}
	}
}
