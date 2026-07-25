namespace WindowsTimerApp;

public enum TimerState { Idle, Running, Paused, Finished }

public class StateManager
{
    public TimerState CurrentState { get; private set; } = TimerState.Idle;

    public void UpdateState(TimerState newState)
    {
        CurrentState = newState;
    }
}
