using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsTimerApp;

public class TrayController : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _contextMenu;

    public TrayController()
    {
        _contextMenu = new ContextMenuStrip();
        _contextMenu.Items.Add("Reset", null, (s, e) => { /* Reset logic */ });
        _contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

        _notifyIcon = new NotifyIcon
        {
            Text = "Windows Timer",
            Icon = SystemIcons.Application,
            ContextMenuStrip = _contextMenu,
            Visible = true
        };
    }

    public void Dispose()
    {
        _notifyIcon.Dispose();
    }
}
