namespace DiceBot.Ui
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.mainConsole = new System.Windows.Forms.TextBox();
			this.statsButton = new System.Windows.Forms.Button();
			this.buttonClear = new System.Windows.Forms.Button();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.operatorLabel = new System.Windows.Forms.Label();
			this.operatorTextBox = new System.Windows.Forms.TextBox();
			this.pluginTabControl = new System.Windows.Forms.TabControl();
			this.statsEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.debugCheckbox = new System.Windows.Forms.CheckBox();
			this.toolTips = new System.Windows.Forms.ToolTip(this.components);
			this.mainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainConsole
			// 
			resources.ApplyResources(this.mainConsole, "mainConsole");
			this.mainConsole.Name = "mainConsole";
			this.mainConsole.ReadOnly = true;
			// 
			// statsButton
			// 
			resources.ApplyResources(this.statsButton, "statsButton");
			this.statsButton.Name = "statsButton";
			this.toolTips.SetToolTip(this.statsButton, resources.GetString("statsButton.ToolTip"));
			this.statsButton.UseVisualStyleBackColor = true;
			// 
			// buttonClear
			// 
			resources.ApplyResources(this.buttonClear, "buttonClear");
			this.buttonClear.Name = "buttonClear";
			this.toolTips.SetToolTip(this.buttonClear, resources.GetString("buttonClear.ToolTip"));
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// mainPanel
			// 
			resources.ApplyResources(this.mainPanel, "mainPanel");
			this.mainPanel.Controls.Add(this.operatorLabel);
			this.mainPanel.Controls.Add(this.operatorTextBox);
			this.mainPanel.Controls.Add(this.pluginTabControl);
			this.mainPanel.Controls.Add(this.statsEnabledCheckBox);
			this.mainPanel.Controls.Add(this.debugCheckbox);
			this.mainPanel.Controls.Add(this.buttonClear);
			this.mainPanel.Controls.Add(this.statsButton);
			this.mainPanel.Name = "mainPanel";
			// 
			// operatorLabel
			// 
			resources.ApplyResources(this.operatorLabel, "operatorLabel");
			this.operatorLabel.Name = "operatorLabel";
			// 
			// operatorTextBox
			// 
			resources.ApplyResources(this.operatorTextBox, "operatorTextBox");
			this.operatorTextBox.Name = "operatorTextBox";
			this.toolTips.SetToolTip(this.operatorTextBox, resources.GetString("operatorTextBox.ToolTip"));
			// 
			// pluginTabControl
			// 
			resources.ApplyResources(this.pluginTabControl, "pluginTabControl");
			this.pluginTabControl.Name = "pluginTabControl";
			this.pluginTabControl.SelectedIndex = 0;
			// 
			// statsEnabledCheckBox
			// 
			resources.ApplyResources(this.statsEnabledCheckBox, "statsEnabledCheckBox");
			this.statsEnabledCheckBox.Checked = true;
			this.statsEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.statsEnabledCheckBox.Name = "statsEnabledCheckBox";
			this.toolTips.SetToolTip(this.statsEnabledCheckBox, resources.GetString("statsEnabledCheckBox.ToolTip"));
			this.statsEnabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// debugCheckbox
			// 
			resources.ApplyResources(this.debugCheckbox, "debugCheckbox");
			this.debugCheckbox.Name = "debugCheckbox";
			this.toolTips.SetToolTip(this.debugCheckbox, resources.GetString("debugCheckbox.ToolTip"));
			this.debugCheckbox.UseVisualStyleBackColor = true;
			// 
			// MainWindow
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mainConsole);
			this.Controls.Add(this.mainPanel);
			this.Name = "MainWindow";
			this.ShowIcon = false;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
			this.Load += new System.EventHandler(this.MainWindow_Load_SetupProperties);
			this.mainPanel.ResumeLayout(false);
			this.mainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox mainConsole;
		private System.Windows.Forms.Button statsButton;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.CheckBox debugCheckbox;
		private System.Windows.Forms.CheckBox statsEnabledCheckBox;
		private System.Windows.Forms.TabControl pluginTabControl;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.Label operatorLabel;
		private System.Windows.Forms.TextBox operatorTextBox;
	}
}

