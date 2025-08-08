@echo off
REM CCXT.NET NuGet Package Publisher Batch File
REM This is a wrapper for the PowerShell script

echo.
echo =========================================
echo   CCXT.NET NuGet Package Publisher
echo =========================================
echo.

REM Check if PowerShell is available
where powershell >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: PowerShell is not installed or not in PATH
    echo Please install PowerShell to continue
    pause
    exit /b 1
)

REM Run the PowerShell script
powershell -ExecutionPolicy Bypass -File "%~dp0publish-nuget.ps1" %*

REM Check the exit code
if %errorlevel% neq 0 (
    echo.
    echo Publishing failed with error code: %errorlevel%
    pause
    exit /b %errorlevel%
)

echo.
echo Done!
pause