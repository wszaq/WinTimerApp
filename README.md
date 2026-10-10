# Windows Timer App

At this review, [GitHub](https://github.com/wszaq/WinTimerApp) did not mark this repository as a fork.

A Windows Forms countdown timer with an always-on-top window and a system tray icon.
The app starts with 3,600 seconds. Start, Pause, Reset, plus, and minus controls adjust the countdown.
See [Program.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/Program.cs#L16-L26) and [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L25-L50).

## Start from source

Use Windows with the .NET 8 SDK. From the repository folder, run:

```powershell
dotnet run --project WindowsTimerApp.csproj
```

The project targets `net8.0-windows` and enables Windows Forms. See [WindowsTimerApp.csproj](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/WindowsTimerApp.csproj#L1-L9).
This setup command was checked against the project file. It was not executed during this documentation review.

## Documentation

- [Getting started](docs/Getting-started.md): build, use, and publish the app.
- [Architecture](docs/Architecture.md): timer, state, form, and tray behavior.
- [Reference](docs/Reference.md): controls, output folders, tests, and current limits.

The tray menu's Reset item has no reset implementation in this snapshot.
Use the window's Reset button. See [TrayController.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TrayController.cs#L12-L16).
