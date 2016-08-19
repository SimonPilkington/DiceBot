using System;
using System.Windows.Forms;

using DiceBot.Ui;

namespace DiceBot
{
	static class Program
	{
		private static System.Threading.Mutex instanceMutex;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			bool mutexCreated;
			instanceMutex = new System.Threading.Mutex(false, "DiceBotInstanceMutex", out mutexCreated);

			if (mutexCreated)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
				AppDomain.CurrentDomain.UnhandledException += (s, e) => UnhandledException((Exception)e.ExceptionObject);
				Application.ThreadException += (s, e) => UnhandledException(e.Exception);
#endif

				MainWindow view = null;
				MainPresenter presenter = null;

				try
				{
					view = new MainWindow();
					// Init the presenter after the view is inited and visible, or it won't be able to show notifications.
					view.Shown += (s, e) => presenter = new MainPresenter(view);

					Application.Run(view);
				}
#if !DEBUG
				catch (Exception x)
				{
					UnhandledException(x);
				}
#endif
				finally
				{
					presenter?.Dispose();
					instanceMutex.Dispose();
				}
			}
			else
			{
				MessageBox.Show("There is already an instance running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private static void UnhandledException(Exception x)
		{
			var result = MessageBox.Show($"A fatal error occurred: {x.Message}\n" + "Write details to error.txt?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

			if (result == DialogResult.Yes)
			{
				using (var writer = new System.IO.StreamWriter("error.txt"))
				{
					writer.WriteLine(x);
				}
			}

			Environment.Exit(1);
		}
	}
}
