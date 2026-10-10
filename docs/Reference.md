# Reference

## Controls

| Control | Effect |
| --- | --- |
| Start | Starts a positive countdown unless it already runs |
| Pause | Pauses a running countdown |
| Reset in window | Returns to 3,600 seconds and Idle |
| Plus | Adds 60 seconds |
| Minus | Subtracts 60 seconds; the result cannot go below zero |
| Exit in tray | Closes the app |
| Reset in tray | Has no implementation in this snapshot |

See [MainForm.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainForm.cs#L40-L50), [TimerEngine.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TimerEngine.cs#L25-L75), and [TrayController.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TrayController.cs#L12-L16).

## Build and package

```sh
dotnet build WindowsTimerApp.csproj
dotnet test WindowsTimerApp.csproj
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish_output
ISCC installer/WindowsTimerApp.iss
```

Use a Windows environment for the Forms app and its tests.
The project references Microsoft.NET.Test.Sdk and xUnit. See [WindowsTimerApp.csproj](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/WindowsTimerApp.csproj#L1-L15).
The publish command is in [publish.sh](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/publish.sh#L1-L2).
The installer step requires Inno Setup's `ISCC` command. See [build_installer.sh](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/build_installer.sh#L1-L9).
The installer writes to `installer_output` and copies `publish_output` files.
See [installer/WindowsTimerApp.iss](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/installer/WindowsTimerApp.iss#L1-L17).

The shell wrapper does not stop explicitly when its publish step fails.
Inspect each command's result before you use the installer output.
This review did not execute these commands or verify an installer.

## Tests and current limits

`TimerEngineTests.cs` and `MainFormTests.cs` contain xUnit tests.
`MainFormVisualTests.cs` captures a form bitmap and compares it with `test_artifacts/visual_regression/baseline/golden.png`.
A missing baseline makes the test create one. A present baseline permits less than 0.1% different pixels.
See [MainFormVisualTests.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/MainFormVisualTests.cs#L15-L47).

The countdown uses UI timer ticks, not a persisted deadline. Closing the app does not save the remaining time.
The state object only stores the current enum value. See [TimerEngine.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/TimerEngine.cs#L14-L23) and [StateManager.cs](https://github.com/wszaq/WinTimerApp/blob/d8cbacf75df1182968c3e78dbee7cd76c1347be7/StateManager.cs#L1-L13).
