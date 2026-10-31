using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WindowsTimerApp;

internal sealed class TrayController : IDisposable
{
    private readonly ContextMenuStrip menu = new();
    private readonly NotifyIcon icon;
    private readonly Icon? appIcon;

    public TrayController(MainWindow window)
    {
        menu.Items.Add("Open timer", null, (_, _) => window.RestoreWindow());
        var cancel = menu.Items.Add("Cancel timer", null, (_, _) => window.CancelCountdown());
        menu.Opening += (_, _) => cancel.Enabled = window.CanCancel;
        menu.Items.Add("Exit", null, (_, _) => window.Close());

        appIcon = Icon.ExtractAssociatedIcon(Environment.ProcessPath!);
        icon = new NotifyIcon { Icon = appIcon ?? SystemIcons.Application, Text = "Shutdown timer", ContextMenuStrip = menu, Visible = true };
        icon.DoubleClick += (_, _) => window.RestoreWindow();
        window.StateChanged += (_, _) => { if (window.WindowState == WindowState.Minimized) window.Hide(); };
        window.DisplayChanged += text => icon.Text = text;
    }

    public void Dispose()
    {
        icon.Dispose();
        menu.Dispose();
        appIcon?.Dispose();
    }
}
