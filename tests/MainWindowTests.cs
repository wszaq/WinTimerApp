using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Controls;
using Xunit;

namespace WindowsTimerApp.Tests;

public class MainWindowTests
{
    internal static void OnUiThread(Action run)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try { run(); }
            catch (Exception ex) { error = ex; }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error != null) ExceptionDispatchInfo.Capture(error).Throw();
    }

    internal static void Click(Button button) => button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

    private sealed class Clock : TimeProvider
    {
        public long Seconds { get; set; }
        public override long TimestampFrequency => 1;
        public override long GetTimestamp() => Seconds;
    }

    [Fact]
    public void PauseResumeCancelAndExpireThroughWindow() => OnUiThread(() =>
    {
        var clock = new Clock();
        int shutdowns = 0;
        var window = new MainWindow(new TimerEngine(3600, clock), () => shutdowns++);
        window.Minutes.Text = "2";
        Click(window.Primary);
        clock.Seconds = 20;
        window.RefreshTimer();
        Assert.Equal("00:01:40", window.Countdown.Text);
        Click(window.Primary);
        Assert.Equal("Paused", window.Status.Text);
        clock.Seconds = 400;
        Click(window.Primary);
        Assert.Equal("Running", window.Status.Text);
        Click(window.CancelTimer);
        clock.Seconds = 1000;
        Assert.False(window.RefreshTimer());
        Assert.Equal(0, shutdowns);
        Click(window.Primary);
        clock.Seconds += 120;
        Assert.True(window.RefreshTimer());
        Assert.False(window.RefreshTimer());
        Assert.Equal(1, shutdowns);
        window.Close();
    });

    [Fact]
    public void InvalidInputBlocksStartAndPresetRestoresIt() => OnUiThread(() =>
    {
        var window = new MainWindow(new TimerEngine(3600), () => throw new Exception());
        foreach (string invalid in new[] { "", "0", "-1", "1441", "abc" })
        {
            window.Minutes.Text = invalid;
            Assert.False(window.Primary.IsEnabled);
        }
        Click((Button)window.Presets.Children[0]);
        Assert.True(window.Primary.IsEnabled);
        Assert.Equal("00:15:00", window.Countdown.Text);
        window.Close();
    });
    [Fact]
    public void MinimizedTimerCanBeRestoredAndClosed() => OnUiThread(() =>
    {
        var engine = new TimerEngine(3600);
        var window = new MainWindow(engine, () => throw new Exception());
        using var tray = new TrayController(window);
        window.Show();
        Click(window.Primary);
        window.WindowState = WindowState.Minimized;
        Assert.False(window.IsVisible);
        Assert.True(window.CanCancel);
        window.RestoreWindow();
        Assert.True(window.IsVisible);
        Assert.Equal("Running", window.Status.Text);
        window.Close();
        Assert.Equal(TimerState.Idle, engine.State);
    });

    [Fact]
    public void FailedShutdownShowsWindowsErrorAndAllowsRestart() => OnUiThread(() =>
    {
        var clock = new Clock();
        var window = new MainWindow(new TimerEngine(3600, clock), () => throw new InvalidOperationException("Access denied"));
        Click(window.Primary);
        clock.Seconds = 3600;
        Assert.True(window.RefreshTimer());
        Assert.Contains("Access denied", window.Detail.Text);
        Click(window.Primary);
        Assert.Equal("Running", window.Status.Text);
        window.Close();
    });}
