using Xunit;
using WindowsTimerApp;

namespace WindowsTimerApp.Tests;

public class TimerEngineTests
{
    [Fact]
    public void Timer_DecrementsSecondsCorrectly()
    {
        // Arrange
        var engine = new TimerEngine(10); // 10 seconds

        // Act
        // We manually trigger a tick for testing purposes
        engine.ManualTick();

        // Assert
        Assert.Equal(9, engine.RemainingSeconds);
    }
}
