@echo off

set "SCRIPT_DIR=%~dp0"
set "CERT_FILE=%SCRIPT_DIR%MauiMoneyMate_0.0.0.0_x64\MauiMoneyMate_0.0.0.0_x64.cer"
set "MSIX_FILE=%SCRIPT_DIR%MauiMoneyMate_0.0.0.0_x64\MauiMoneyMate_0.0.0.0_x64.msix"

REM Import certificate to "Trusted People" store on local machine
certutil -addstore "TrustedPeople" "%CERT_FILE%"

REM Run the .msix file
start "" "%MSIX_FILE%"

echo Installation completed.