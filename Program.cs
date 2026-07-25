using System;
using System.Windows.Forms;

namespace WindowsTimerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("Starting application...");
                ApplicationConfiguration.Initialize();

                Console.WriteLine("Initializing StateManager...");
                var stateManager = new StateManager();

                Console.WriteLine("Initializing TimerEngine...");
                var engine = new TimerEngine(3600, stateManager); // Default 60 minutes

                Console.WriteLine("Initializing TrayController...");
                using var trayController = new TrayController();

                Console.WriteLine("Launching MainForm...");
                Application.Run(new MainForm(engine, trayController));

                Console.WriteLine("Application exited successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CRITICAL ERROR: {ex}");
                Environment.Exit(1);
            }
        }
    }
}
