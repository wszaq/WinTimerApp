# Contributing

Thank you for your interest in this project.

## Report a bug

1. Open the [issues page](https://github.com/wszaq/WinTimerApp/issues).
2. Search for an existing issue that describes the same problem.
3. If none exists, open a new issue. Include the app version, the Windows version, the steps to reproduce the problem, and what you expected to happen.

Do not report security problems in a public issue. See [SECURITY.md](SECURITY.md).

## Propose a change

1. Open an issue that describes the change. Wait for a response before you write large code.
2. Fork the repository and create a branch for your change.
3. Make the change. Keep it small and limit it to the problem.
4. Run the checks in the next section.
5. Open a pull request. Describe what changed and how you tested it.

## Run the checks

On Windows, install the .NET 8 SDK. From the repository folder, run these commands.

```powershell
dotnet build WindowsTimerApp.csproj
dotnet test tests/Timer.Tests.csproj
```

Run both commands before you open a pull request.
