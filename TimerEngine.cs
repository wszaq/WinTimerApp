namespace WindowsTimerApp;

public class TimerEngine
{
    public int RemainingSeconds { get; set; }
    public event Action? OnTick;
    public event Action? OnTimerFinished;
    private readonly StateManager _stateManager;

    public TimerEngine(int seconds, StateManager stateManager)
    {
        RemainingSeconds = seconds;
        _stateManager = stateManager;
        _stateManager.UpdateState(TimerState.Idle);
    }

    public void Start()
    {
        if (RemainingSeconds > 0)
        {
            _stateManager.UpdateState(TimerState.Running);
        }
    }

    public void Pause()
    {
        if (_stateManager.CurrentState == TimerState.Running)
        {
            _stateManager.UpdateState(TimerState.Paused);
        }
    }

    public void ManualTick()
    {
        if (RemainingSeconds > 0)
        {
            RemainingSeconds--;
            OnTick?.Invoke();
            if (RemainingSeconds == 0)
            {
                _stateManager.UpdateState(TimerState.Finished);
                OnTimerFinished?.Invoke();
            }
        }
    }

    public void Reset(int seconds)
    {
        RemainingSeconds = seconds;
        _stateManager.UpdateState(TimerState.Idle);
        OnTick?.Invoke();
    }
}
