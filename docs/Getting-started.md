# Getting started

This tutorial is for a Windows user who wants to run the timer from source.
The source defines the commands below. This review did not build or run the Windows app.

## Run the timer

1. Install the .NET 8 SDK on Windows.
2. Open a terminal in the repository folder.
3. Run:

```powershell
dotnet run --project WindowsTimerApp.csproj
```

The project requires Windows Forms and targets `net8.0-windows`. See [WindowsTimerApp.csproj](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/WindowsTimerApp.csproj#L1-L9).
The startup code creates a timer with 3,600 seconds. See [Program.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/Program.cs#L16-L26).

4. Select **Start** to start the countdown.
5. Select **Pause** to pause it.
6. Select **Start** to continue.
7. Select **Reset** to return to 3,600 seconds.

The plus and minus buttons change the time by 60 seconds.
The window remains above other windows. See [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L25-L50).
At zero, the app displays a message and changes the tray text.
See [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L89-L94).
Use the tray menu's **Exit** item to close the app. See [TrayController.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TrayController.cs#L12-L24).

## Publish a Windows executable

From the repository folder, run:

```sh
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish_output
```

This is the exact command in [publish.sh](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/publish.sh#L1-L2).
It selects 64-bit Windows and includes the runtime in `publish_output`.
The installer expects `WindowsTimerApp.exe` in this output. See [installer/WindowsTimerApp.iss](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/installer/WindowsTimerApp.iss#L11-L17).

For installer tools and tests, read [Reference](Reference.md).
