# Windows shutdown timer

Choose when Windows shuts down. The default duration is 60 minutes. Select Start shutdown timer to begin. You can pause, resume, or cancel it. Minimize the window to keep the timer running in the system tray. Closing the app cancels the timer.

![Shutdown timer window](test_artifacts/visual_regression/baseline/golden.png)

## Run

On Windows, install the .NET 8 SDK and run this command from the repository folder.

```powershell
dotnet run --project WindowsTimerApp.csproj
```

See [Getting started](docs/Getting-started.md) for the controls, [Reference](docs/Reference.md) for tests and packaging, and [Architecture](docs/Architecture.md) for the timer logic.
