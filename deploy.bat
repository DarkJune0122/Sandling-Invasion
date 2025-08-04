@echo false

set "root=%cd%"
set "deploy=%root%\deploy"
set "file=SandlingInvasion.dll"
set "zip=SandlingInvasion.zip"

:: Remove "README" overwriting if you want mod to have description, different from the one on Github.
xcopy /y "%root%\README.md" "%deploy%"
xcopy /y "%root%\icon.png" "%deploy%"
xcopy /y "%root%\manifest.json" "%deploy%"
xcopy /y "%root%\bin\Debug\%file%" "%deploy%"

:: WinRAR's "a" = add files to archive
:: -afzip = force ZIP format (default is RAR)
:: -ep1 = exclude base folder, include only contents
:: -r = recursive
winrar a -afzip -ep1 -r "%zip%" "%deploy%\*"