[Setup]
AppName=Windows Timer App
AppVersion=1.0
DefaultDirName={autopf}\Windows Timer App
DefaultGroupName=Windows Timer App
UninstallDisplayIcon={app}\WindowsTimerApp.exe
Compression=lzma
SolidCompression=yes
OutputDir=../installer_output

[Files]
; Source from our published directory, destination to the app folder
Source: "../publish_output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Windows Timer App"; Filename: "{app}\WindowsTimerApp.exe"
Name: "{autodesktop}\Windows Timer App"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Components]
; No extra components needed for this .iss file.

[Run]
Filename: "{app}\WindowsTimerApp.exe"; Description: "{cm:LaunchProgram,Windows Timer App}"; Flags: nowait postinstall skipifsilent
