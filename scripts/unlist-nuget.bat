@echo off
REM CCXT.NET NuGet Package Unlist Batch File
REM This is a wrapper for the PowerShell unlist script

echo.
echo =========================================
echo   CCXT.NET NuGet Package Unlist Tool
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

REM Prompt for version if not provided
set /p VERSION="Enter the package version to unlist (e.g., 1.1.7): "

REM Run the PowerShell script
powershell -ExecutionPolicy Bypass -File "%~dp0unlist-nuget.ps1" -Version "%VERSION%"

REM Check the exit code
if %errorlevel% neq 0 (
    echo.
    echo Unlist operation failed with error code: %errorlevel%
    pause
    exit /b %errorlevel%
)

echo.
echo Done!
pause