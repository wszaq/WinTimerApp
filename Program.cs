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

            using var trayController = new TrayController();

            // Placeholder for future Main Window launch
            Application.Run(new Form());
        }
    }
}
