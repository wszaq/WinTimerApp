using System;
using System.Windows.Forms;

namespace WindowsTimerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var stateManager = new StateManager();
            var engine = new TimerEngine(10, stateManager); // Default 10s timer
            using var trayController = new TrayController();

            // Wire up Engine -> State transitions if needed (already partially handled in Engine)
            // For this task, the main thing is connecting everything.

            Application.Run(new MainForm(engine, trayController));
        }
    }
}
