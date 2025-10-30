@echo off
echo Building AppBundle Avalonia - Modern Cross-Platform Installer...
echo.

:: Check if .NET SDK is available
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ❌ .NET SDK not found. Please install .NET 8 SDK.
    pause
    exit /b 1
)

echo ✅ .NET SDK found, proceeding with build...
echo.

echo Building single-file executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Build successful!
    echo ✅ Output: bin\Release\net8.0-windows\win-x64\publish\AppBundle.Avalonia.exe
    echo.
    
    :: Get file size
    for %%I in (bin\Release\net8.0-windows\win-x64\publish\AppBundle.Avalonia.exe) do set size=%%~zI
    echo File size: %size% bytes (~%size:~0,-6% MB)
    echo.
    
    echo You can now run AppBundle.Avalonia.exe (requires admin rights for installation)
    echo.
    echo ✅ Ready to install apps: Chrome, VLC, Microsoft 365, 7-Zip, RustDesk, Zoom
) else (
    echo ❌ Build failed!
)

pause