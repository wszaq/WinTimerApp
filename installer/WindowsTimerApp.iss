[Setup]
AppName=Windows Timer App
AppVersion=1.0
DefaultDirName={autopf}\Windows Timer App
DefaultGroupName=Windows Timer App
UninstallDisplayIcon={app}\WindowsTimerApp.exe
Compression=lzma
SolidCompression=yes
OutputDir=../installer_output
OutputBaseFilename=setup

[Files]
; Package only the self-contained application.
Source: "..\publish_single_file\WindowsTimerApp.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Windows Timer App"; Filename: "{app}\WindowsTimerApp.exe"
Name: "{autodesktop}\Windows Timer App"; Filename: "{app}\WindowsTimerApp.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Components]
; No extra components needed for this .iss file.

[Run]
Filename: "{app}\WindowsTimerApp.exe"; Description: "{cm:LaunchProgram,Windows Timer App}"; Flags: nowait postinstall skipifsilent
