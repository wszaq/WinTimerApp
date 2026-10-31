# Getting started

## Open the app

1. On a PC with 64-bit Windows (x64), [download WindowsTimerApp.exe](https://github.com/wszaq/WinTimerApp/releases/latest/download/WindowsTimerApp.exe).
2. Open the downloaded file. The timer starts in the **Ready** state with 60 minutes selected.

To check a download, run this PowerShell command in the download folder. Compare the result with the SHA-256 digest for the executable on the [release page](https://github.com/wszaq/WinTimerApp/releases/latest).

```powershell
Get-FileHash .\WindowsTimerApp.exe -Algorithm SHA256
```

## Set a shutdown time

1. Choose **15 min**, **30 min**, **1 hour**, or **2 hours**. For a different duration, enter 1 to 1440 whole minutes in **Custom minutes**.
2. Save your work. Select **Start shutdown timer**. The window shows the remaining time and the planned shutdown time.
3. To stop the countdown temporarily, select **Pause timer**. Select **Resume timer** to continue from the remaining time.
4. To cancel the shutdown, select **Cancel timer** before the countdown ends. The app returns to **Ready**. You can then set a different duration.

At zero, the app requests a Windows shutdown without another prompt from the timer.

## Use the system tray

Minimize the window to keep the timer running in the system tray. Double-click the tray icon to open the window again. You can also right-click the icon and select **Open timer**, **Cancel timer**, or **Exit**. Closing the app cancels the countdown.
