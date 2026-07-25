using System;
using System.Windows.Forms;

namespace WindowsTimerApp;

public class TimerEngine
{
    public int RemainingSeconds { get; set; }
    public event Action? OnTick;
    public event Action? OnTimerFinished;
    private readonly StateManager _stateManager;
    private readonly System.Windows.Forms.Timer _timer;

    public TimerEngine(int seconds, StateManager stateManager)
    {
        RemainingSeconds = seconds;
        _stateManager = stateManager;
        _stateManager.UpdateState(TimerState.Idle);

        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 1000; // 1 second
        _timer.Tick += (s, e) => ManualTick();
    }

    public void Start()
    {
        if (RemainingSeconds > 0 && _stateManager.CurrentState != TimerState.Running)
        {
            _stateManager.UpdateState(TimerState.Running);
            _timer.Start();
        }
    }

    public void Pause()
    {
        if (_stateManager.CurrentState == TimerState.Running)
        {
            _stateManager.UpdateState(TimerState.Paused);
            _timer.Stop();
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
                _timer.Stop();
                OnTimerFinished?.Invoke();
            }
        }
    }

    public void Reset(int seconds)
    {
        RemainingSeconds = seconds;
        _stateManager.UpdateState(TimerState.Idle);
        _timer.Stop();
        OnTick?.Invoke();
    }

    public void AdjustTime(int delta)
    {
        RemainingSeconds = Math.Max(0, RemainingSeconds + delta);
        OnTick?.Invoke();
        if (RemainingSeconds == 0 && _stateManager.CurrentState != TimerState.Idle)
        {
            _stateManager.UpdateState(TimerState.Finished);
            _timer.Stop();
            OnTimerFinished?.Invoke();
        }
    }
}
