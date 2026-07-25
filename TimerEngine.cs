namespace WindowsTimerApp;

public class TimerEngine
{
    public int RemainingSeconds { get; set; }
    public event Action? OnTick;
    public event Action? OnTimerFinished;

    public TimerEngine(int seconds)
    {
        RemainingSeconds = seconds;
    }

    public void ManualTick()
    {
        if (RemainingSeconds > 0)
        {
            RemainingSeconds--;
            OnTick?.Invoke();
            if (RemainingSeconds == 0)
            {
                OnTimerFinished?.Invoke();
            }
        }
    }
}
