namespace DiceBot.Ui
{
	partial class HistogramDialog
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			this.histogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.histogram)).BeginInit();
			this.SuspendLayout();
			// 
			// histogram
			// 
			chartArea1.AxisX.Interval = 1D;
			chartArea1.Name = "MainArea";
			this.histogram.ChartAreas.Add(chartArea1);
			this.histogram.Dock = System.Windows.Forms.DockStyle.Fill;
			legend1.Alignment = System.Drawing.StringAlignment.Center;
			legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend1.Name = "Legend1";
			this.histogram.Legends.Add(legend1);
			this.histogram.Location = new System.Drawing.Point(0, 0);
			this.histogram.Name = "histogram";
			this.histogram.Size = new System.Drawing.Size(784, 361);
			this.histogram.TabIndex = 0;
			this.histogram.Text = "Histogram";
			// 
			// HistogramWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 361);
			this.Controls.Add(this.histogram);
			this.Name = "HistogramWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Histogram";
			this.Load += new System.EventHandler(this.HistogramWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.histogram)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart histogram;
	}
}