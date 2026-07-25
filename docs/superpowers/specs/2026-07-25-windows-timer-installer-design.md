# Inno Setup Installer Design Document

**Date:** 2026-07-25
**Status:** Approved
**Topic:** Windows Timer App - Inno Setup Installation Wizard

## Overview

The goal is to create a professional, lightweight, and easy-to-use installer for the Windows Timer App using **Inno Setup**. The installer will bundle the published .NET application into a single `setup.exe` that provides a standard Windows installation experience.

## Architecture & Payload

### Payload
The installer wraps the compiled output of a `dotnet publish -c Release -r win-x64 --self-contained true` command. This ensures the application is fully self-contained and does not require a separate .NET runtime installation on the target machine.
*   **Primary Executable**: `WindowsTimerApp.exe`
*   **Included Assets**: All necessary `.dll` files and configuration files produced by the publish command.

### Installation Target
*   **Default Directory**: `%ProgramFiles%\Windows Timer App`
*   **Installation Type**: Standard file extraction to a local directory.

## User Experience (The Wizard)

The installer will follow a classic, non-intrusive wizard flow:

1.  **Welcome Screen**: Introduction to the Windows Timer App.
2.  **License Agreement**: Displaying a brief usage terms text.
3.  **Destination Selection**: Standard dialog for choosing the installation path.
4.           **Installation Progress**: A visual progress bar showing the extraction of files from the package.
5.   **Completion Screen**: Final summary with an option to "Launch Windows Timer App" immediately upon finishing.

## System Integration

### Shortcuts & Access
*   **Start Menu**: Creates a shortcut in `Start Menu\Windows Timer App`.
*   **Desktop Shortcut**: (Optional) An unchecked checkbox during the wizard allows the user to add a desktop shortcut.

### Uninstallation
The installer will leverage Inno Setup's built-in uninstaller capability, registering the application with Windows so it can be cleanly removed via **"Apps & Features"** or the **Control Panel**. This ensures all files and shortcuts are wiped from the system without leaving remnants.

## Implementation Strategy

### Build Pipeline
1.  **Step 1 (Pre-build)**: Execute `dotnet publish` to generate the production-ready, self-contained folder.
2.  **Step MM (Build)**: Run the `ISCC` (Inno Setup Compiler) command using a `.iss` script that references the published folder.
3.  **Step 3 (Output)**: The final artifact will be a single `WindowsTimerApp_Setup.exe`.

### Verification Plan
*   **Installation Test**: Running the `setup.exe` on a clean environment and verifying files land in `%ProgramFiles%`.
*   **Functionality Test**: Launching the app via the newly created Start Menu shortcut to ensure it runs without errors.
*   **Uninstallation Test**: Running the uninstaller and confirming the application directory and shortcuts are removed.
