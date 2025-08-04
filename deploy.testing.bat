@echo false

set "root=%cd%"
set "deploy=C:\Users\Dark\AppData\Roaming\r2modmanPlus-local\ETG\profiles\Default\BepInEx\plugins\SandlingInvasion"
set "file=SandlingInvasion.dll"
set "zip=SandlingInvasion.zip"

:: Remove "README" overwriting if you want mod to have description, different from the one on Github.
xcopy /y "%root%\README.md" "%deploy%"
xcopy /y "%root%\icon.png" "%deploy%"
xcopy /y "%root%\manifest.json" "%deploy%"
xcopy /y "%root%\bin\Debug\%file%" "%deploy%"