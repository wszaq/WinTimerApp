using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using Xunit;

namespace WindowsTimerApp.Tests;

public class MainFormTests
{
    [Fact]
    public void MainForm_InitialState_HasCorrectLabelText()
    {
        // Arrange
        var engine = new TimerEngine(10, new StateManager());
        var tray = new TrayController();
        var form = new MainForm(engine, tray);

        // Act: Use reflection to access the private _timeLabel
        var fieldInfo = typeof(MainForm).GetField("_timeLabel", BindingFlags.NonPublic | BindingFlags.Instance);
        var label = fieldInfo?.GetValue(form) as Label;

        // Assert
        Assert.NotNull(label);
        Assert.Equal("10s", label.Text);
        Assert.Equal(Color.White, label.ForeColor);
    }

    [Fact]
    public void MainForm_ControlsAreAddedToForm()
    {
        // Arrange
        var engine = new TimerEngine(10, new StateManager());
        var tray = new TrayController();
        var form = new MainForm(engine, tray);

        // Act & Assert: Check that we have the expected number of controls (Label + 3 Buttons)
        Assert.Equal(4, form.Controls.Count);
    }
}
