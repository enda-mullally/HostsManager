;
; Hosts Maneger InnoSetup Installer Script.
; Copyright (c) 2022-2024 Enda Mullally.
;

#ifndef InstallerVersion
  #define InstallerVersion "0.0.0"
#endif

#define public Dependency_NoExampleSetup
#include "CodeDependencies.iss"

#define DotNetVersionBuildDir "net8.0-windows7.0"

[Setup]
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
AppId=Hosts_Manager_10e26e4d
AppVerName=Hosts Manager v{#InstallerVersion}
UninstallDisplayName=Hosts Manager {#InstallerVersion}
OutputBaseFilename=EM.HostsManager.Installer.{#InstallerVersion}
UninstallDisplayIcon={app}\EM.HostsManager.App.exe
WizardStyle=modern
AppName=Hosts Manager
AppPublisher=Enda Mullally
DefaultDirName={commonpf}\Hosts Manager
DefaultGroupName=Hosts Manager
PrivilegesRequired=admin
LicenseFile=License.txt
AppVersion={#InstallerVersion}
AppCopyright=Copyright 2021-2024 Enda Mullally
DisableProgramGroupPage=true
DisableDirPage=true
Compression=lzma2
SetupIconFile=HostsManager.ico
WizardSmallImageFile=SetupIcon.bmp
AlwaysRestart=no

[Icons]
Name: "{group}\Hosts Manager"; Filename: "{app}\EM.HostsManager.App.exe"

[Registry]
; Application name, version etc are Global. Should only change on upgrade etc.
Root: HKLM; SubKey: "Software\Enda Mullally"; ValueType: string; ValueName: ""; ValueData: ""; Flags: uninsdeletekey
Root: HKLM; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "App"; ValueData: "{app}\EM.HostsManager.App.exe"; Flags: uninsdeletekey
Root: HKLM; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "Version"; ValueData: "{#InstallerVersion}"; Flags: uninsdeletekey

; Note app settings - stored in the HKCU hive. - Disbaled - Now managed by the application
; Root: HKCU; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "FirstRun"; ValueData: "true"; Flags: uninsdeletekey

; Start the app on user startup (minimized to the system tray) - Disbaled - Now managed by the application
; Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Hosts Manager"; ValueData: """{app}\EM.HostsManager.App.exe"" /min"; Flags: uninsdeletekey

[Files]
Source: "License.txt"; DestDir: {app}; Flags: ignoreversion noencryption
Source: "Licenses-ThirdParty.txt"; DestDir: {app}; Flags: ignoreversion noencryption
Source: "HostsManager.ico"; DestDir: {app}; Flags: ignoreversion noencryption  
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\EM.HostsManager.App.exe"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\EM.HostsManager.App.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\EM.HostsManager.Infrastructure.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\EM.HostsManager.App.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\Microsoft.Extensions.DependencyInjection.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\{#DotNetVersionBuildDir}\Microsoft.Extensions.DependencyInjection.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption

; Deps
Source: "netcorecheck_x64.exe"; Flags: dontcopy noencryption

; Currently 8.0.3
Source: "windowsdesktop-runtime-8.0.3-win_x64.exe"; Flags: dontcopy noencryption

; Clean-up previous file name(s).
[InstallDelete]
Type: files; Name: "{app}\EM.HostsManager.Infrastructure.dll"

[Run]
Filename: {app}\EM.HostsManager.App.exe; Description: {cm:LaunchProgram,{cm:AppName}}; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{app}\EM.HostsManager.App.exe"; Parameters:"/uninstall"; RunOnceId: "Hosts_Manager_10e26e4d"; Flags: waituntilterminated

[CustomMessages]
AppName=Hosts Manager
LaunchProgram=Start Hosts Manager v{#InstallerVersion}

[Code]
function InitializeSetup: Boolean;
begin
  // We depend on the .NET 8.0 Desktop runtime so install it if needed (x64).
  // Note:
  //   This is an embedded offline install, see how 'windowsdesktop-runtime-8.*.*-win_x64.exe' is
  //   packed above and extracted below.
  //   This will make our installer much larger in size, but,
  //   will work well on machines that are offline or behind paranoid corporate firewalls.
  // Note:
  //   This may be obvious, but, if the user already has a compatible .NET 8.0 Desktop runtime
  //   installed everything above will be skipped and the installer will just install Hosts Manager.
  //
  // Special thanks to all the contributors @ https://github.com/DomGries/InnoDependencyInstaller !

  ExtractTemporaryFile('windowsdesktop-runtime-8.0.3-win_x64.exe');

  Dependency_AddDotNet80Desktop;
  
  // ...

  Result := True;
end;

// Added to ensure that uninstall doesn't ask to restart
function UninstallNeedRestart(): Boolean;
begin
  Result := False;
end;