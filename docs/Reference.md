# Build and package

Use the .NET 8 SDK on Windows. Run these commands from the repository folder.

## Build and test

```powershell
dotnet build WindowsTimerApp.csproj
dotnet test tests/Timer.Tests.csproj
```

The automated tests use a substitute shutdown action. They can check expiration without shutting down the test PC.

## Check a downloaded build

1. Launch the executable and select **15 min**. Start the timer and check that the remaining time decreases.
2. Pause the timer for a few seconds. Check that the display holds its value. Resume the timer.
3. Minimize the window. Open it again from the tray icon and check that the countdown still runs.
4. Select **Cancel timer**. Check that the state returns to **Ready** with 15 minutes selected. Close the window.

## Publish a standalone executable

```powershell
dotnet publish WindowsTimerApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -o publish_single_file
```

Distribute `publish_single_file/WindowsTimerApp.exe`. It includes the .NET runtime. In Git Bash, `bash publish.sh` runs the same publish step.

## Build an installer

Install Inno Setup on Windows and add its compiler, `ISCC.exe`, to your PATH. In Git Bash, run the wrapper.

```sh
bash build_installer.sh
```

The script publishes the executable and packages it in `installer_output/setup.exe`.
