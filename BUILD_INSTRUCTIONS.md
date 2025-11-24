# Building the Supermarket Application as Standalone EXE

## Option 1: Using the Build Script (Easiest)

1. **Install .NET 8.0 SDK** (one-time setup)
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - Install the SDK (not just the runtime)
   - Restart your command prompt after installation

2. **Run the build script**
   - Double-click `BUILD.bat`
   - Wait for the build to complete

3. **Get your executable**
   - Find `SupermarketApp.exe` in the `Release` folder
   - This is a standalone executable that can run on any Windows PC without prerequisites

## Option 2: Manual Build via Command Line

Open Command Prompt in the project folder and run:

```cmd
dotnet restore SupermarketApp/SupermarketApp.csproj
dotnet publish SupermarketApp/SupermarketApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ./Release
```

## Option 3: Using Visual Studio

1. Open `SupermarketApp.sln` in Visual Studio 2022
2. Right-click on the SupermarketApp project
3. Select "Publish"
4. Choose "Folder" as the target
5. Configure:
   - Target Runtime: win-x64
   - Deployment Mode: Self-contained
   - Produce single file: Yes
6. Click "Publish"

## What You Get

- **Single EXE file**: `SupermarketApp.exe` (approximately 70-100 MB)
- **No installation required**: Just copy and run on any Windows 10/11 PC
- **No .NET runtime needed**: Everything is bundled in the executable
- **Database included**: SQLite database is created automatically on first run

## Distribution

Simply copy `SupermarketApp.exe` to any Windows computer and double-click to run. The application will:
- Create its database file (`supermarket.db`) in the same folder
- Work immediately without any setup or installation

## System Requirements

- Windows 10 or Windows 11 (64-bit)
- No other software required

## Troubleshooting

**If you don't have .NET SDK installed:**
- You need it only for building, not for running the final EXE
- Download from: https://dotnet.microsoft.com/download/dotnet/8.0
- Choose "SDK" not "Runtime"

**If the build fails:**
- Make sure you have internet connection (for downloading packages)
- Try running Command Prompt as Administrator
- Check that all source files are present

**If the EXE doesn't run on target PC:**
- Make sure it's Windows 10 or 11 (64-bit)
- Try running as Administrator
- Check Windows Defender isn't blocking it
