# Architecture

## Startup and ownership

`Program` creates `StateManager`, `TimerEngine`, and `TrayController`.
It passes the engine and tray controller to `MainForm`. The app disposes the tray controller when it finishes.
See [Program.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/Program.cs#L16-L28).

## Timer and state

`StateManager` holds one of four states: Idle, Running, Paused, or Finished.
See [StateManager.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/StateManager.cs#L1-L13).
`TimerEngine` uses a Windows Forms timer with a 1,000-millisecond interval.
Start and Pause change both the state and the timer. A tick reduces the remaining seconds.
At zero, the engine stops the timer and raises `OnTimerFinished`.
See [TimerEngine.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TimerEngine.cs#L14-L55).

Reset changes the state to Idle. AdjustTime clamps the remaining time to zero or above.
An adjustment to zero can finish an active timer. See [TimerEngine.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TimerEngine.cs#L58-L75).
The source stores state in memory. `StateManager` has no save or restore operation.

## UI and tray

`MainForm` connects button clicks to engine methods. Engine ticks update the display and tray text.
See [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L40-L50) and [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L74-L94).
The tray menu supports Exit. Its Reset handler contains only a comment.
See [TrayController.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TrayController.cs#L12-L24).

The window has no standard border and remains on top. Mouse handlers allow a drag from the form.
See [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L25-L30) and [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L59-L72).

## Verification boundary

The repository contains engine, form, and screenshot tests.
The screenshot test compares pixels with a stored baseline. See [MainFormVisualTests.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainFormVisualTests.cs#L23-L48).
This documentation review inspected the implementation. It did not verify Windows event timing or visual behavior.
Read [Reference](Reference.md) for the test command and limits.
