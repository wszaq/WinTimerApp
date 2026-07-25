using Xunit;
using WindowsTimerApp;

namespace WindowsTimerApp.Tests;

public class TimerEngineTests
{
    [Fact]
    public void Timer_DecrementsSecondsCorrectly()
    {
        // Arrange
        var stateManager = new StateManager();
        var engine = new TimerEngine(10, stateManager); // 10 seconds

        // Act
        engine.ManualTick();

        // Assert
        Assert.Equal(9, engine.RemainingSeconds);
    }

    [Fact]
    public void Timer_UpdatesStateToRunningWhenStarted()
    {
        // Arrange
        var stateManager = new StateManager();
        var engine = new TimerEngine(10, stateManager);

        // Act
        engine.Start();

        // Assert
        Assert.Equal(TimerState.Running, stateManager.CurrentState);
    }

    [Fact]
    public void Timer_UpdatesStateToFinishedWhenDone()
    {
        // Arrange
        var stateManager = new StateManager();
        var engine = new TimerEngine(1, stateManager);
        engine.Start();

        // Act
        engine.ManualTick();

        // Assert
        Assert.Equal(TimerState.Finished, stateManager.CurrentState);
    }
}
