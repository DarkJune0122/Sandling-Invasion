@echo off

set "root=%cd%"
set "deploy=%root%\deploy"
set "file=SandlingInvasion.dll"
set "zip=SandlingInvasion.zip"

:: Where to auto-deploy mod for testing.
set "modFolder=C:\Users\Dark\AppData\Roaming\r2modmanPlus-local\ETG\profiles\Default\BepInEx\plugins"
set "modName=SandlingInvasion"

:: Delete deploy folder if it exists
if exist "%deploy%" (
	rmdir /s /q "%deploy%"
)

:: Delete ZIP file if it exists
if exist "%zip%" (
	del /f /q "%zip%"
)

:: Recreate deploy folder
mkdir "%deploy%"

:: Remove "README" overwriting if you want mod to have description, different from the one on Github.
xcopy /y "%root%\README.md" "%deploy%"
xcopy /y "%root%\CHANGELOG.md" "%deploy%"
xcopy /y "%root%\icon.png" "%deploy%"
xcopy /y "%root%\manifest.json" "%deploy%"
xcopy /y "%root%\bin\Debug\%file%" "%deploy%"
call "%root%\move.bat" -target "%root%\plugins" -destination "%deploy%\plugins"

:: WinRAR's "a" = add files to archive
:: -afzip = force ZIP format (default is RAR)
:: -ep1 = exclude base folder, include only contents
:: -r = recursive
start "" /wait winrar a -afzip -ep1 -r "%root%\%zip%" "%deploy%\*"

:: Test mod deployment.
if exist "C:\Users\Dark" (
	:: Auto-deploy zip as an internal mod for testing.
	if exist "%modFolder%\%modName%" (
		rmdir /s /q "%modFolder%\%modName%"
	)
	
	mkdir "%modFolder%\%modName%"
	start "" /wait winrar x -r -o+ "%root%\%zip%" "%modFolder%\%modName%"
	
	if exist "%modFolder%\%modName%\plugins" (
		xcopy "%modFolder%\%modName%\plugins\*" "%modFolder%\%modName%" /E /H /Y
		rmdir /s /q "%modFolder%\%modName%\plugins"
	)
)