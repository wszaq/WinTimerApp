# Design Document: Windows Timer App

**Date:** 2026-07-25
**Status:** Approved
**Topic:** Minimalist Windows System Utility Timer

## Overview

The goal is to create a lightweight, unobtrusive, and high-performance timer utility for Windows. The application will follow a "one countdown at a time" philosophy, focusing on simplicity and minimal system footprint. It will provide a hybrid user experience via a small, semi-transparent window and a persistent system tray presence.

## Architecture

The application is built using **C# / .NET (WinForms)** for native performance and seamless integration with Windows APIs.

### Core Components

1.  **Timer Engine (Logic)**:
    *   A backend component using `System.Timers.Timer`.
    *   Handles the fundamental countdown logic (decrementing seconds).
    *   Triggers events for state changes (`OnTick`, `OnTimerFinished`).
    *   Operates independently of the UI to ensure timing accuracy even if the window is minimized.

2.  **Main Window (UI)**:
    *   A lightweight, borderless or minimal-border WinForms window.
    *   Features a clear text display for the remaining time.
/ Controls: Simple buttons for Start, Pause, and Reset.
    *   Properties: `TopMost = true` for "always on top" capability; supports semi-transparency.

3.  **System Tray Controller**:
    *   Uses `NotifyIcon` to provide a persistent presence in the Windows System Tray.
    *   Provides a right-click context menu (Pause, Reset, Exit).
    *   Handles the "Time's Up" visual indicator via icon changes.

4.  **State Manager**:
    *   Acts as the single source of truth for the application state (`Running`, `Paused`, `Finished`).
    *   Synchronizes the `Timer Engine` with both the `Main Window` and the `System Tray Controller`.

## Data Flow & Interaction

### The Countdown Loop
*   **Trigger**: User interaction (Start/Pause) or initial time setting.
*   **Update Cycle**: Every second, the `Engine` decrements the counter and broadcasts an `OnTick` event.
*   **Synchronization**: Both the UI Label and the Tray Tooltip subscribe to `OnTick`, ensuring a consistent view across all interfaces.

### User Interaction Scenarios
*   **Setting the Timer**: Users can set duration via window controls or tray context menu.
*   **Start/Pause/Reset**: Direct interaction with the small window or tray menu toggles the engine state.
*   **The "Time's Up" Event**:
    *   When the counter reaches zero, `OnTimerFinished` is triggered.
    *   **Visual Alert**: The system tray icon changes (e.g., from a clock to an alert symbol) to notify the user without intrusive popups.

### Error Handling & Edge Cases
*   **Invalid Input**: Non-numeric or negative time inputs are ignored by the `State Manager`.
*    **Application Exit**: A clean shutdown procedure is triggered via the tray menu, ensuring all timers and threads are disposed of correctly.

## Testing Strategy

### Unit Testing (Engine)
*   Verify correct second-by-second decrementing.
*   Validate `OnTimerFinished` triggers precisely at zero.
*   Ensure pause/resume logic does not cause time drift or skips.

### Integration Testing (Flow)
*   Verify the `Main Window` text updates in sync with `Engine` ticks.
*   Confirm that tray menu actions correctly command the `Engine`.

### Manual Verification (UX)
*   Test "Always on Top" behavior and transparency levels.
*   Manually verify the visual change of the tray icon upon timer completion.
