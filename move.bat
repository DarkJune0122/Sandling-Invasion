@echo off

: GETOPTS
if /i "%~1"=="" ( goto :SENDBUILD )
if /i "%~1"=="-t" ( set "target=%~2" & shift & shift & goto :GETOPTS)
if /i "%~1"=="-target" ( set "target=%~2" & shift & shift & goto :GETOPTS)
if /i "%~1"=="-d" ( set "destination=%~2" & shift & shift & goto :GETOPTS)
if /i "%~1"=="-destination" ( set "destination=%~2" & shift & shift & goto :GETOPTS)
echo Unrecognizable variable of type %~1 provided! Exiting.
exit /b

: SENDBUILD
setlocal EnableDelayedExpansion

:: Loop through all PNG files (recursively)
for /R "%target%" %%F in (*.png) do (
    :: Get relative path
	set "REL=%%F"
	set "REL=!REL:%target%=!"
	
	:: Get destination path with subfolders
	set "DEST_PATH=%destination%!REL!"
	set "DEST_DIR=!DEST_PATH!\.."

	:: Create the destination folder if needed
	if not exist "!DEST_DIR!" (
		mkdir "!DEST_DIR!"
	)

	:: Copy file while keeping folder structure (use move if desired)
	copy "%%F" "!DEST_PATH!" >nul
)