namespace DiceBot.Ui
{
	partial class StatsWindow
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
			this.usersComboBox = new System.Windows.Forms.ComboBox();
			this.statsDataGrid = new System.Windows.Forms.DataGridView();
			this.userLabel = new System.Windows.Forms.Label();
			this.datePicker = new System.Windows.Forms.DateTimePicker();
			this.dateLabel = new System.Windows.Forms.Label();
			this.dieRowStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.viewUserHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.statsDataGrid)).BeginInit();
			this.dieRowStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// usersComboBox
			// 
			this.usersComboBox.CausesValidation = false;
			this.usersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.usersComboBox.FormattingEnabled = true;
			this.usersComboBox.Location = new System.Drawing.Point(49, 12);
			this.usersComboBox.Name = "usersComboBox";
			this.usersComboBox.Size = new System.Drawing.Size(121, 21);
			this.usersComboBox.TabIndex = 0;
			// 
			// statsDataGrid
			// 
			this.statsDataGrid.AllowUserToAddRows = false;
			this.statsDataGrid.AllowUserToDeleteRows = false;
			this.statsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.statsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.statsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.statsDataGrid.Location = new System.Drawing.Point(12, 39);
			this.statsDataGrid.Name = "statsDataGrid";
			this.statsDataGrid.ReadOnly = true;
			this.statsDataGrid.Size = new System.Drawing.Size(718, 433);
			this.statsDataGrid.TabIndex = 1;
			this.statsDataGrid.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.statsDataGrid_CellContextMenuStripNeeded);
			// 
			// userLabel
			// 
			this.userLabel.AutoSize = true;
			this.userLabel.Location = new System.Drawing.Point(11, 15);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(32, 13);
			this.userLabel.TabIndex = 2;
			this.userLabel.Text = "User:";
			// 
			// datePicker
			// 
			this.datePicker.Checked = false;
			this.datePicker.CustomFormat = "dd/MM/yyyy H:00";
			this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.datePicker.Location = new System.Drawing.Point(244, 12);
			this.datePicker.Name = "datePicker";
			this.datePicker.ShowCheckBox = true;
			this.datePicker.Size = new System.Drawing.Size(200, 20);
			this.datePicker.TabIndex = 3;
			this.datePicker.ValueChanged += new System.EventHandler(this.datePicker_ValueChanged);
			// 
			// dateLabel
			// 
			this.dateLabel.AutoSize = true;
			this.dateLabel.Location = new System.Drawing.Point(176, 15);
			this.dateLabel.Name = "dateLabel";
			this.dateLabel.Size = new System.Drawing.Size(62, 13);
			this.dateLabel.TabIndex = 4;
			this.dateLabel.Text = "Cutoff date:";
			// 
			// dieRowStrip
			// 
			this.dieRowStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.viewUserHistogramToolStripMenuItem,
			this.viewHistogramToolStripMenuItem});
			this.dieRowStrip.Name = "contextMenuStrip1";
			this.dieRowStrip.ShowImageMargin = false;
			this.dieRowStrip.Size = new System.Drawing.Size(157, 70);
			// 
			// viewUserHistogramToolStripMenuItem
			// 
			this.viewUserHistogramToolStripMenuItem.Name = "viewUserHistogramToolStripMenuItem";
			this.viewUserHistogramToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.viewUserHistogramToolStripMenuItem.Text = "View user histogram";
			this.viewUserHistogramToolStripMenuItem.Click += new System.EventHandler(this.viewUserHistogramToolStripMenuItem_Click);
			// 
			// viewHistogramToolStripMenuItem
			// 
			this.viewHistogramToolStripMenuItem.Name = "viewHistogramToolStripMenuItem";
			this.viewHistogramToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.viewHistogramToolStripMenuItem.Text = "View histogram";
			this.viewHistogramToolStripMenuItem.Click += new System.EventHandler(this.viewHistogramToolStripMenuItem_Click);
			// 
			// StatsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 484);
			this.Controls.Add(this.dateLabel);
			this.Controls.Add(this.datePicker);
			this.Controls.Add(this.userLabel);
			this.Controls.Add(this.statsDataGrid);
			this.Controls.Add(this.usersComboBox);
			this.Name = "StatsWindow";
			this.ShowIcon = false;
			this.Text = "Statistics";
			this.Load += new System.EventHandler(this.StatsWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.statsDataGrid)).EndInit();
			this.dieRowStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox usersComboBox;
		private System.Windows.Forms.DataGridView statsDataGrid;
		private System.Windows.Forms.Label userLabel;
		private System.Windows.Forms.DateTimePicker datePicker;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.ContextMenuStrip dieRowStrip;
		private System.Windows.Forms.ToolStripMenuItem viewUserHistogramToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewHistogramToolStripMenuItem;
	}
}