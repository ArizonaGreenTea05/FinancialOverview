@echo off

:: BatchGotAdmin
:-------------------------------------
REM  --> Check for permissions
    IF "%PROCESSOR_ARCHITECTURE%" EQU "amd64" (
> nul 2>&1 "%SYSTEMROOT%\SysWOW64\cacls.exe" "%SYSTEMROOT%\SysWOW64\config\system"
) ELSE (
> nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
)

REM --> If error flag set, we do not have admin.
    IF '%errorlevel%' NEQ '0' (
        echo Requesting administrative privileges...
        goto UACPrompt
    ) ELSE ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"

    "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    if exist "%temp%\getadmin.vbs" (
        del "%temp%\getadmin.vbs"
    )

:: Actual script starts here

set "SCRIPT_DIR=%~dp0"
set "CERT_FILE=%SCRIPT_DIR%MauiMoneyMate_0.0.0.0_x64\MauiMoneyMate_0.0.0.0_x64.cer"
set "MSIX_FILE=%SCRIPT_DIR%MauiMoneyMate_0.0.0.0_x64\MauiMoneyMate_0.0.0.0_x64.msix"

REM Import certificate to "Trusted People" store on local machine
certutil -addstore "TrustedPeople" "%CERT_FILE%"

REM Run the .msix file
start "" "%MSIX_FILE%"

echo Installation completed.