using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WindowsTimerApp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var app = new Application();
        var window = new MainWindow(new TimerEngine(3600), ShutDown);
        using var tray = new TrayController(window);
        app.Run(window);
    }

    private static void ShutDown()
    {
        using var command = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.SystemDirectory, "shutdown.exe"),
            Arguments = "/s /t 0",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true
        }) ?? throw new InvalidOperationException("Windows could not start the shutdown command.");
        string error = command.StandardError.ReadToEnd().Trim();
        command.WaitForExit();
        if (command.ExitCode != 0)
            throw new InvalidOperationException(error.Length > 0 ? error : $"Windows returned error {command.ExitCode}.");
    }
}
