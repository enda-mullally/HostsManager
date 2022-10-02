;
; Hosts Maneger InnoSetup Installer Script.
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
Root: HKLM; SubKey: "Software\Enda Mullally"; ValueType: string; ValueName: ""; ValueData: ""; Flags: uninsdeletekey
Root: HKLM; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "App"; ValueData: "{app}\EM.HostsManager.App.exe"; Flags: uninsdeletekey
Root: HKLM; SubKey: "Software\Enda Mullally\Hosts Manager"; ValueType: string; ValueName: "Version"; ValueData: "{#InstallerVersion}"; Flags: uninsdeletekey

[Files]
; Files from Installer and local folders (shared)
; Note any dll files merged via ILMerge.exe do not need to be added here. 
Source: "License.txt"; DestDir: {app}; Flags: ignoreversion noencryption
Source: "HostsManager.ico"; DestDir: {app}; Flags: ignoreversion noencryption  
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.exe"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.pdb"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.dll"; DestDir: "{app}"; Flags: ignoreversion noencryption
Source: "..\EM.HostsManager.App\bin\Release\net6.0-windows\EM.HostsManager.App.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion noencryption

; Deps
Source: "netcorecheck_x64.exe"; Flags: dontcopy noencryption

; Currently 6.0.9
Source: "dotnet60desktop_x64.exe"; Flags: dontcopy noencryption

[Run]
;Filename: "{app}\{#AppExe}"; Parameters:"/installsilent"; Description: ""; Flags: shellexec skipifsilent

[UninstallRun]
Filename: "{app}\EM.HostsManager.App.exe"; Parameters:"/quit"; RunOnceId: "Hosts_Manager_10e26e4d"; Flags: waituntilterminated

[Code]
function InitializeSetup: Boolean;
begin
  ExtractTemporaryFile('dotnet60desktop_x64.exe');

  // add the dependencies you need
  Dependency_AddDotNet60Desktop;
  // ...

  Result := True;
end;

// Added to ensure that uninstall asks to restart, needed to cleanup the
// ShellExtensions successfully.
function UninstallNeedRestart(): Boolean;
begin
  Result := False;
end;
