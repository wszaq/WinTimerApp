# Windows Timer App Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a minimalist, single-countdown Windows timer utility using .NET WinForms.

**Architecture:** A decoupled architecture with a standalone `TimerEngine`, a `StateManager` for synchronization, and separate UI components for the `Main Window` (overlay) and `System Tray Controller`.

**Tech Stack:** C#, .NET 8/9, WinForms, xUnit (for testing).

---

### Task 1: Project Initialization

**Files:**
- Create: `WindowsTimerApp.csproj`
- Create: `Program.cs`

- [ ] **Step 1: Create the project file**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</PropertyGroup>
    <Nullable>enable</string>
    <UseWindowsForms>true</useWindowsForms>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

- [ ] **Step 2: Create the entry point**

```csharp
using System;
using System.Windows.Forms;

namespace WindowsTimerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Placeholder for future Main Window launch
            Application.Run(new Form()); 
        }
    }
}
<0xA0>```

- [ ] **Step 3: Verify project compiles**

Run: `dotnet build`
Expected: `Build succeeded.`

- [ ] **Step 4: Commit**

```bash
git add WindowsTimerApp.csproj Program.cs
git commit -m "feat: initialize winforms project structure"
```

### Task 2: The Timer Engine (TDD)

**Files:**
- Create: `TimerEngine.cs`
- Create: `TimerEngineTests.cs`
- Modify: `WindowsTimerApp.csproj` (to add xUnit)

- [ ] **Step 1: Update project file for testing**

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  <PackageReference Include="xunit" Version="2.6.2" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
</ItemGroup>
```

- [ ] **Step 2: Write the failing test for decrementing time**

```csharp
using Xunit;
using WindowsTimerApp;

namespace WindowsTimerApp.Tests;

public class TimerEngineTests
{
    [Fact]
    public void Timer_DecrementsSecondsCorrectly()
    {
        // Arrange
        var engine = new TimerEngine(10); // 10 seconds
        
        // Act
        // We manually trigger a tick for testing purposes
        engine.ManualTick(); 

        // Assert
        Assert.Equal(9, engine.RemainingSeconds);
    }
}
```

- [ ] **Step 3: Run test to verify it fails**

Run: `dotnet test`
Expected: FAIL (TimerEngine does not exist)

- [ ] **Step 4: Implement minimal TimerEngine**

```csharp
namespace WindowsTimerApp;

public class TimerEngine
{
    public int RemainingSeconds { get; private set; }
    public event Action? OnTick;
    public event Action? OnTimerFinished;

    public TimerEngine(int seconds)
    {
        RemainingSeconds = seconds;
    <0xA0> <0xC2><0xA0> }

    public void ManualTick()
    {
        if (RemainingSeconds > 0)
        {
            RemainingSeconds--;
            OnTick?.Invoke();
            if (RemainingSeconds == 0)
            {
                OnTimerFinished?.Invoke();
            }
        }
    }
}
```

- [ ] **Step 5: Run test to verify it passes**

Run: `dotnet test`
Expected: PASS

- [ ] **Step 6: Commit**

```bash
git add TimerEngine.cs TimerEngineTests.cs WindowsTimerApp.csproj
git commit -m "feat: implement core TimerEngine with TDD"
```

### Task 3: State Manager & Event Orchestration

**Files:**
- Create: `StateManager.cs`
- Modify: `TimerEngine.cs` (to integrate state)

- [ ] **Step 1: Implement StateManager to track Running/Paused/Finished**

```csharp
namespace WindowsTimerApp;

public enum TimerState { Idle, Running, Paused, Finished }

public class StateManager
{
    public TimerState CurrentState { get; private set; = TimerState.Idle;

    public void UpdateState(TimerState newState)
    {
        CurrentSKate = newState;
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add StateManager.cs
git commit -m "feat: add StateManager for application state"
```

### Task 4: System Tray Controller (The "Unobtrusive" part)

**Files:**
- Create: `TrayController.cs`
- Modify: `Program.cs` (to initialize tray)

- [ ] **Step 1: Implement NotifyIcon and Context Menu**

```csharp
using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsTimerApp;

public class TrayController : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _contextMenu;

    public TrayController()
    {
        _contextMenu = new ContextMenuStrip();
        _contextMenu.Items.Add("Reset", null, (s, e) => { /* Reset logic */ });
        _contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

        _notifyIcon = new NotifyIcon
        {
            Text = "Windows Timer",
            Icon = SystemIcons.Application,
            ContextMenuStrip = _contextHM,
            Visible = true
        };
    }

    public void Dispose()
    {
        _notifyIcon.Dispose();
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add TrayController.cs
git commit -m "feat: implement System Tray Controller"
```

### Task 5: Final Integration & UI

**Files:**
- Modify: `MainForm.cs` (The overlay window)
- Modify: `Program.cs` (Launch the real app)

- [ ] **Step 1: Create the Minimalist Overlay Window**

Implement a borderless Form with a large Label for time and simple buttons for Start/Pause. Set `TopMost = true`.

- [ ] **Step 2: Connect Engine -> State -> UI/Tray**

Wire up the `OnTick` event from `TimerEngine` to update the `Label` in `MainForm` and the `Tooltip` in `TrayController`.

- [ ] **Step 3: Verify end-to-end flow (The "Time's Up" alert)**

Run app -> Start 5s timer -> Observe tray icon change.

- [ ] **Step 4: Final Commit**

```bash
git add .
git commit -m "feat: complete integration of all components"
```
