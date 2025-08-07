@echo on

set "root=%cd%"
set "deploy=%root%\deploy"
set "file=SandlingInvasion.dll"
set "zip=SandlingInvasion.zip"

if exist "%deploy%" (
	del /y "%deploy%"
)

if exist "%root%\%zip%" (
	del /y "%root%\%zip%"
)

:: Remove "README" overwriting if you want mod to have description, different from the one on Github.
xcopy /y "%root%\README.md" "%deploy%"
xcopy /y "%root%\icon.png" "%deploy%"
xcopy /y "%root%\manifest.json" "%deploy%"
xcopy /y "%root%\bin\Debug\%file%" "%deploy%"
call "%root%\move.bat" -target "%root%\plugin" -destination "%deploy%"

:: WinRAR's "a" = add files to archive
:: -afzip = force ZIP format (default is RAR)
:: -ep1 = exclude base folder, include only contents
:: -r = recursive
winrar a -afzip -ep1 -r "%zip%" "%deploy%\*"

:: Test mod deployment.
if exist "C:\Users\Dark" (
	:: Auto-deploy zip as an internal mod for testing.
	winrar x -r -o+ "%zip%" "C:\Users\Dark\AppData\Roaming\r2modmanPlus-local\ETG\profiles\Default\BepInEx\plugins\SandlingInvasion"
)