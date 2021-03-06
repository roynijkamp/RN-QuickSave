; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "RN QuicSave"
#define MyAppVersion "1.0.0.0"
#define MyAppFileVersion GetFileVersion("K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\bin\Debug\RN-QuickSave.dll")  
#define MyAppPublisher "Roy Nijkamp"
#define MyAppURL "https://www.roynijkamp.nl"
#define MyAppExeName "RN-QuicSave"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D4FE5436-8F76-4FA7-AA8A-23A1FB27D59F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppFileVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=C:\ProgramData\Autodesk\ApplicationPlugins\RN-QuickSave.bundle
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\Distribution
OutputBaseFilename=setup {#MyAppName} {#MyAppVersion}
SetupIconFile=K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\Distribution\logo.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"

[Files]
;DLL laden om te checken of AutoCAD draait
Source: "K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\Distribution\psvince.dll"; Flags: dontcopy
;DLL opslaan om te kunnen gebruiken bij uninstall
Source: "K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\Distribution\psvince.dll"; Destdir: "{app}"; Flags: ignoreversion
;programma
Source: "K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\Distribution\PackageContents.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\bin\Debug\RN-QuickSave.dll"; DestDir: "{app}\Contents"; Flags: ignoreversion
Source: "K:\RoyN_doc\Documenten\Visual Studio 2015\Projects\RN-QuickSave\RN-QuickSave\bin\Debug\RNCeckItem.dll"; DestDir: "{app}\Contents"; Flags: ignoreversion


; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
// function IsModuleLoaded to call at install time
// added also setuponly flag
function IsModuleLoaded(modulename: String ):  Boolean;
external 'IsModuleLoaded@files:psvince.dll stdcall setuponly';

// function IsModuleLoadedU to call at uninstall time
// added also uninstallonly flag
function IsModuleLoadedU(modulename: String ):  Boolean;
external 'IsModuleLoaded@{app}\psvince.dll stdcall uninstallonly' ;


function InitializeSetup(): Boolean;
begin

  // check if autocad is running
  if IsModuleLoaded( 'acad.exe' ) then
  begin
    MsgBox( 'U heeft AutoCAD niet afgesloten! Sluit AutoCAD af en open de Setup opnieuw.',
             mbError, MB_OK );
    Result := false;
  end
  else Result := true;
end;

function InitializeUninstall(): Boolean;
begin

  // check if autocad is running
  if IsModuleLoadedU( 'acad.exe' ) then
  begin
    MsgBox( 'U heeft AutoCAD niet afgesloten! Sluit AutoCAD af en open de Setup opnieuw.',
             mbError, MB_OK );
    Result := false;
  end
  else Result := true;

  // Unload the DLL, otherwise the dll psvince is not deleted
  UnloadDLL(ExpandConstant('{app}\psvince.dll'));

end;

