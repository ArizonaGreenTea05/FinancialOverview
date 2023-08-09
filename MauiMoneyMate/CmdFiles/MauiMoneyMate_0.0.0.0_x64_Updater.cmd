@echo off

REM Before publishing: Copy this file into the same directory as the .msix file and rename this file so the version matches

:: Actual script starts here
set "SCRIPT_NAME=%~n0"
set "SCRIPT_BASENAME=%SCRIPT_NAME:_updater=%"
set "SCRIPT_DIR=%~dp0"
set "MSIX_FILE=%SCRIPT_DIR%\%SCRIPT_BASENAME%.msix"

REM Run the .msix file
start "" "%MSIX_FILE%"