@echo off
echo ========================================
echo Building Supermarket Application
echo ========================================
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK is not installed on this PC!
    echo.
    echo ========================================
    echo SOLUTION: Build on GitHub (No Installation Needed)
    echo ========================================
    echo.
    echo Since you cannot install software on this PC,
    echo you can build the application using GitHub's free service.
    echo.
    echo See GITHUB_BUILD_GUIDE.md for step-by-step instructions.
    echo.
    echo Quick steps:
    echo 1. Create free GitHub account
    echo 2. Upload this project
    echo 3. GitHub builds it automatically
    echo 4. Download the ready-to-use EXE
    echo.
    echo No installation required on your PC!
    echo.
    pause
    exit /b 1
)

echo Restoring packages...
dotnet restore SupermarketApp/SupermarketApp.csproj
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo Building self-contained executable...
dotnet publish SupermarketApp/SupermarketApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ./Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo BUILD SUCCESSFUL!
    echo ========================================
    echo.
    echo The executable is located at:
    echo Release\SupermarketApp.exe
    echo.
    echo You can copy this file to any Windows PC and run it without installing anything.
    echo.
) else (
    echo.
    echo ========================================
    echo BUILD FAILED!
    echo ========================================
    echo.
)

pause
