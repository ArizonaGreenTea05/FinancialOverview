@echo off

REM Before publishing: Copy this file in the .zip file that contains the MauiMoneyMate_x.x.x.x_x64 folder and rename this file so the version matches

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
set "SCRIPT_NAME=%~n0"
set "SCRIPT_BASENAME=%SCRIPT_NAME:_installer=%"
set "SCRIPT_DIR=%~dp0"
set "CERT_FILE=%SCRIPT_DIR%%SCRIPT_BASENAME%\%SCRIPT_BASENAME%.cer"
set "MSIX_FILE=%SCRIPT_DIR%%SCRIPT_BASENAME%\%SCRIPT_BASENAME%.msix"

REM Import certificate to "Trusted People" store on local machine
certutil -addstore "TrustedPeople" "%CERT_FILE%"

REM Run the .msix file
start "" "%MSIX_FILE%"

echo Installation completed.