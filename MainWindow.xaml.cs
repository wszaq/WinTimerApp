using System;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WindowsTimerApp;

public partial class MainWindow : Window
{
    private readonly TimerEngine engine;
    private readonly Action requestShutdown;
    private readonly DispatcherTimer pulse = new() { Interval = TimeSpan.FromMilliseconds(200) };
    private string? result;
    private int selectedMinutes = 60;
    private bool validDuration = true;
    private bool ready;

    internal event Action<string>? DisplayChanged;
    internal bool CanCancel => engine.State is TimerState.Running or TimerState.Paused;

    internal MainWindow(TimerEngine engine, Action requestShutdown)
    {
        this.engine = engine;
        this.requestShutdown = requestShutdown;
        InitializeComponent();
        ready = true;
        Minutes.Text = selectedMinutes.ToString(CultureInfo.CurrentCulture);
        pulse.Tick += (_, _) => RefreshTimer();
        Closed += (_, _) => { pulse.Stop(); engine.Reset(selectedMinutes * 60); };
        Render();
    }

    private void MinutesChanged(object sender, TextChangedEventArgs e)
    {
        if (!ready || CanCancel) return;
        validDuration = int.TryParse(Minutes.Text, NumberStyles.None, CultureInfo.CurrentCulture, out int value)
                        && value is >= 1 and <= 1440;
        result = null;
        if (validDuration)
        {
            selectedMinutes = value;
            engine.Reset(value * 60);
        }
        Render();
    }

    private void PresetClick(object sender, RoutedEventArgs e) =>
        SetMinutes(int.Parse((string)((Button)sender).Tag, CultureInfo.InvariantCulture));

    private void MinusClick(object sender, RoutedEventArgs e) => SetMinutes(Math.Max(1, selectedMinutes - 1));
    private void PlusClick(object sender, RoutedEventArgs e) => SetMinutes(Math.Min(1440, selectedMinutes + 1));

    private void SetMinutes(int value) => Minutes.Text = value.ToString(CultureInfo.CurrentCulture);

    private void PrimaryClick(object sender, RoutedEventArgs e)
    {
        if (!validDuration || RefreshTimer()) return;
        if (engine.State == TimerState.Running)
        {
            engine.Pause();
            if (RefreshTimer()) return;
            pulse.Stop();
        }
        else
        {
            if (engine.State == TimerState.Finished) engine.Reset(selectedMinutes * 60);
            result = null;
            engine.Start();
            pulse.Start();
        }
        Render();
    }

    private void CancelClick(object sender, RoutedEventArgs e) => CancelCountdown();

    internal void CancelCountdown()
    {
        pulse.Stop();
        engine.Reset(selectedMinutes * 60);
        result = "Timer canceled. Windows will stay on.";
        Render();
    }

    internal bool RefreshTimer()
    {
        if (engine.Refresh()) { Finish(); return true; }
        if (engine.State == TimerState.Running) Render();
        return false;
    }

    private void Finish()
    {
        pulse.Stop();
        try
        {
            requestShutdown();
            result = "Shutdown requested.";
        }
        catch (Exception ex)
        {
            result = $"Shutdown failed. {ex.Message}";
            RestoreWindow();
        }
        Render();
    }

    internal void RestoreWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private void Render()
    {
        var state = engine.State;
        var remaining = TimeSpan.FromSeconds(engine.RemainingSeconds);
        string time = $"{(int)remaining.TotalHours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
        Countdown.Text = validDuration ? time : "--:--:--";
        Status.Text = state switch
        {
            TimerState.Running => "Running",
            TimerState.Paused => "Paused",
            TimerState.Finished => "Ended",
            _ => "Ready"
        };
        Detail.Text = !validDuration ? "Enter 1 to 1440 minutes." : result ?? state switch
        {
            TimerState.Running => $"Windows will shut down at {DateTime.Now.Add(remaining):t}.",
            TimerState.Paused => "Resume the timer to set a new shutdown time.",
            _ => "Windows will shut down when the timer ends."
        };
        Primary.Content = state switch
        {
            TimerState.Running => "Pause timer",
            TimerState.Paused => "Resume timer",
            _ => "Start shutdown timer"
        };
        Primary.IsEnabled = validDuration;
        DurationControls.IsEnabled = !CanCancel;
        CancelTimer.Visibility = CanCancel ? Visibility.Visible : Visibility.Collapsed;
        Progress.Value = validDuration ? 100d * engine.RemainingSeconds / (selectedMinutes * 60) : 0;
        foreach (var preset in Presets.Children.OfType<Button>())
        {
            if (validDuration && preset.Tag.ToString() == selectedMinutes.ToString(CultureInfo.InvariantCulture))
                preset.SetResourceReference(Control.BorderBrushProperty, "Signal");
            else
                preset.ClearValue(Control.BorderBrushProperty);
        }
        DisplayChanged?.Invoke($"Shutdown timer · {Status.Text} · {time}");
    }
}
