;
; Hosts Maneger InnoSetup Installer Script.
; Copyright (c) 2022 Enda Mullally.
;

#include <C:\Program Files (x86)\Inno Download Plugin\idp.iss>

#ifndef InstallerVersion
  #define InstallerVersion "0.0.0"
#endif

#define public Dependency_NoExampleSetup
#include "CodeDependencies.iss"

[Setup]
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
AppId=Hosts_Manager_10e26e4d
AppVerName=Hosts Manager - Version: {#InstallerVersion}
OutputBaseFilename=EM.HostsManager.Installer.{#InstallerVersion}
UninstallDisplayIcon={app}\EM.HostsManager.App.exe
WizardStyle=modern
AppName=Hosts Manager
AppPublisher=Enda Mullally
DefaultDirName={commonpf}\Enda Mullally\Hosts Manager
DefaultGroupName=Hosts Manager
PrivilegesRequired=admin
LicenseFile=License.txt
AppVersion={#InstallerVersion}
AppCopyright=Copyright 2021-2022 Enda Mullally
DisableProgramGroupPage=true
DisableDirPage=true
Compression=lzma2
SetupIconFile=HostsManager.ico
WizardSmallImageFile=SetupIcon.bmp
AlwaysRestart=no

[Icons]
Name: "{group}\Hosts Manager"; Filename: "{app}\EM.HostsManager.App.exe"

[Registry]
; AppName
Root: HKCU; SubKey: "Software\Enda Mullally"; ValueType: string; ValueName: ""; ValueData: ""; Flags: uninsdeletekey
Root: HKCU; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "App"; ValueData: "{app}\EM.HostsManager.App.exe"; Flags: uninsdeletekey
Root: HKCU; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "Version"; ValueData: "{#InstallerVersion}"; Flags: uninsdeletekey
Root: HKCU; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "FirstRun"; ValueData: "true"; Flags: uninsdeletekey

; Start the app on current user startup (minimized to the system tray)
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Hosts Manager"; ValueData: """{app}\EM.HostsManager.App.exe"" /min"; Flags: uninsdeletekey

[Files]
Source: "License.txt"; DestDir: {app}; Flags: ignoreversion noencryption
Source: "HostsManager.ico"; DestDir: {app}; Flags: ignoreversion noencryption  
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.exe"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion noencryption

; Deps
Source: "netcorecheck_x64.exe"; Flags: dontcopy noencryption

; Currently 6.0.9
Source: "dotnet60desktop_x64.exe"; Flags: dontcopy noencryption

[Run]
Filename: {app}\EM.HostsManager.App.exe; Description: {cm:LaunchProgram,{cm:AppName}}; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{app}\EM.HostsManager.App.exe"; Parameters:"/quit"; RunOnceId: "Hosts_Manager_10e26e4d"; Flags: waituntilterminated

[CustomMessages]
AppName=Hosts Manager
LaunchProgram=Start Hosts Manager v{#InstallerVersion} now

[Code]
function InitializeSetup: Boolean;
begin
  // We depend on the .NET 6.0 Desktop runtime so install it if needed (x64).
  // Note:
  //   This is an embedded offline install, see how 'dotnet60desktop_x64.exe' is
  //   packed above and extracted below.
  //   This will make our installer much larger in size, but,
  //   will work well on machines that are offline or behind paranoid corporate firewalls.
  // Note:
  //   This may be obvious, but, if the user already has a compatible .NET 6.0 Desktop runtime
  //   installed everything above will be skipped and the installer will just install Hosts Manager.
  //
  // Special thanks to all the contributors @ https://github.com/DomGries/InnoDependencyInstaller !

  ExtractTemporaryFile('dotnet60desktop_x64.exe');

  Dependency_AddDotNet60Desktop;
  
  // ...

  Result := True;
end;

// Added to ensure that uninstall doesn't ask to restart
function UninstallNeedRestart(): Boolean;
begin
  Result := False;
end;
