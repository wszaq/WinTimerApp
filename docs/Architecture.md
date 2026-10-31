# Architecture

[Program.cs](../Program.cs) creates the WPF window, timer engine, and system tray icon. It supplies the window with a shutdown action that runs the system copy of `shutdown.exe /s /t 0`. The window displays an error if the command fails.

[TimerEngine.cs](../TimerEngine.cs) measures elapsed time with `TimeProvider`. The engine keeps the remaining duration in memory. Pause preserves partial seconds. Resume starts a new elapsed-time interval. Reset cancels the countdown.

[MainWindow.xaml.cs](../MainWindow.xaml.cs) refreshes the display with a WPF dispatcher timer. When a refresh first detects expiration, the window requests shutdown once. Tests can supply a simulated clock and a substitute shutdown action.

[TrayController.cs](../TrayController.cs) owns the Windows Forms notification icon. Minimizing hides the window. Closing the window stops the timer and exits the app.
