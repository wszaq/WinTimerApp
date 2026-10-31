using Xunit;

namespace WindowsTimerApp.Tests;

public class TimerEngineTests
{
    private sealed class TestClock : TimeProvider
    {
        private long timestamp;
        public override long TimestampFrequency => TimeSpan.TicksPerSecond;
        public override long GetTimestamp() => timestamp;
        public void Advance(int milliseconds) => timestamp += TimeSpan.FromMilliseconds(milliseconds).Ticks;
    }

    [Fact]
    public void LateRefreshExpiresOnce()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(10, clock);
        timer.Start();
        clock.Advance(3300);
        Assert.False(timer.Refresh());
        Assert.Equal(7, timer.RemainingSeconds);
        clock.Advance(15000);
        Assert.True(timer.Refresh());
        Assert.Equal(0, timer.RemainingSeconds);
        Assert.Equal(TimerState.Finished, timer.State);
        Assert.False(timer.Refresh());
    }

    [Fact]
    public void PauseAndResumePreservePartialSeconds()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(2, clock);
        timer.Start();
        clock.Advance(650);
        timer.Pause();
        clock.Advance(60000);
        Assert.False(timer.Refresh());
        Assert.Equal(TimerState.Paused, timer.State);
        Assert.Equal(2, timer.RemainingSeconds);
        timer.Start();
        clock.Advance(1349);
        Assert.False(timer.Refresh());
        Assert.Equal(1, timer.RemainingSeconds);
        clock.Advance(1);
        Assert.True(timer.Refresh());
    }

    [Fact]
    public void ResetCancelsDeadlineAndAllowsNewTimer()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(5, clock);
        timer.Start();
        timer.Reset(3);
        clock.Advance(9000);
        Assert.False(timer.Refresh());
        Assert.Equal(TimerState.Idle, timer.State);
        Assert.Equal(3, timer.RemainingSeconds);
        timer.Start();
        clock.Advance(3000);
        Assert.True(timer.Refresh());
    }

    [Fact]
    public void PauseAfterDeadlineStillLetsRefreshReportExpiration()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(1, clock);
        timer.Start();
        clock.Advance(1000);
        timer.Pause();
        Assert.True(timer.Refresh());
    }

    [Fact]
    public void RepeatedStartDoesNotDelayDeadline()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(5, clock);
        timer.Start();
        clock.Advance(4000);
        timer.Start();
        clock.Advance(1000);
        Assert.True(timer.Refresh());
    }

    [Fact]
    public void AdjustRespectsElapsedTimeAndNeverReturnsNegativeDuration()
    {
        var clock = new TestClock();
        var timer = new TimerEngine(5, clock);
        timer.AdjustTime(-60);
        timer.Start();
        Assert.Equal(TimerState.Idle, timer.State);
        timer.Reset(5);
        timer.Start();
        clock.Advance(1500);
        timer.AdjustTime(2);
        Assert.Equal(6, timer.RemainingSeconds);
        clock.Advance(5499);
        Assert.False(timer.Refresh());
        clock.Advance(1);
        Assert.True(timer.Refresh());
    }
}
