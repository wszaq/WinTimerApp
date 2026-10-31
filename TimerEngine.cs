using System;

namespace WindowsTimerApp;

public sealed class TimerEngine
{
    private readonly TimeProvider clock;
    private TimeSpan duration;
    private long startedAt;

    public TimerEngine(int seconds, TimeProvider? clock = null)
    {
        this.clock = clock ?? TimeProvider.System;
        Reset(seconds);
    }

    public TimerState State { get; private set; }
    public int RemainingSeconds => (int)Math.Ceiling(Remaining.TotalSeconds);

    private TimeSpan Remaining
    {
        get
        {
            if (State != TimerState.Running)
                return duration;

            var remaining = duration - clock.GetElapsedTime(startedAt);
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    public void Start()
    {
        if (State == TimerState.Running || duration == TimeSpan.Zero)
            return;

        startedAt = clock.GetTimestamp();
        State = TimerState.Running;
    }

    public void Pause()
    {
        if (State != TimerState.Running)
            return;

        var remaining = Remaining;
        if (remaining == TimeSpan.Zero)
            return;

        duration = remaining;
        State = TimerState.Paused;
    }

    public void Reset(int seconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(seconds);
        duration = TimeSpan.FromSeconds(seconds);
        State = TimerState.Idle;
    }

    public bool Refresh()
    {
        if (State != TimerState.Running || Remaining > TimeSpan.Zero)
            return false;

        duration = TimeSpan.Zero;
        State = TimerState.Finished;
        return true;
    }

    public void AdjustTime(int seconds)
    {
        var ticks = Remaining.Ticks + TimeSpan.FromSeconds(seconds).Ticks;
        duration = TimeSpan.FromTicks(Math.Clamp(ticks, 0, TimeSpan.FromSeconds(int.MaxValue).Ticks));
        startedAt = clock.GetTimestamp();
        if (duration == TimeSpan.Zero || State == TimerState.Finished)
            State = TimerState.Idle;
    }
}
