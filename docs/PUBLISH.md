# Publishing CCXT.NET to NuGet

This guide explains how to publish the CCXT.NET package to NuGet.org.

## Prerequisites

1. **.NET SDK** - Install from https://dotnet.microsoft.com/download
2. **NuGet API Key** - Get from https://www.nuget.org/account/apikeys
3. **PowerShell** - Pre-installed on Windows

## Quick Start

### Method 1: Using Batch File (Simplest)

1. Navigate to the `scripts` folder
2. Double-click `publish-nuget.bat`
3. Enter your NuGet API key when prompted
4. Confirm publication

### Method 2: Using PowerShell with Parameters

```powershell
# From project root
.\scripts\publish-nuget.ps1 -ApiKey "YOUR_API_KEY_HERE"

# Dry run (test without publishing)
.\scripts\publish-nuget.ps1 -ApiKey "YOUR_API_KEY_HERE" -DryRun

# Skip tests (faster publishing)
.\scripts\publish-nuget.ps1 -ApiKey "YOUR_API_KEY_HERE" -SkipTests

# Skip build step (use existing build)
.\scripts\publish-nuget.ps1 -ApiKey "YOUR_API_KEY_HERE" -SkipBuild

# Skip both build and tests
.\scripts\publish-nuget.ps1 -ApiKey "YOUR_API_KEY_HERE" -SkipBuild -SkipTests
```

### Method 3: Using Environment Variable

```powershell
# Set API key in environment
$env:NUGET_API_KEY = "YOUR_API_KEY_HERE"

# Run without parameters (from project root)
.\scripts\publish-nuget.ps1
```

### Method 4: Using Configuration File

1. Copy `scripts\nuget-config.ps1.example` to `scripts\nuget-config.ps1`
2. Edit `scripts\nuget-config.ps1` and add your API key
3. Run the publisher:

```powershell
# Load configuration and publish (from project root)
. .\scripts\nuget-config.ps1
.\scripts\publish-nuget.ps1
```

## What the Script Does

1. **Validates Environment** - Checks for .NET SDK and API key
2. **Cleans Previous Packages** - Removes old .nupkg files from bin\Release
3. **Builds Project** - Compiles in Release mode (skip with `-SkipBuild`)
4. **Runs Tests** - Ensures all tests pass (skip with `-SkipTests`)
5. **Creates Package** - Generates .nupkg file in bin\Release folder
6. **Publishes to NuGet** - Uploads to nuget.org (skip with `-DryRun`)

## Version Management

Before publishing, update the version in `src/ccxt.net.csproj`:

```xml
<Version>1.5.2</Version>
<AssemblyVersion>1.5.2.15</AssemblyVersion>
<FileVersion>1.5.2.15</FileVersion>
```

## Security Notes

⚠️ **IMPORTANT**: 
- Never commit your API key to version control
- Add `scripts/nuget-config.ps1` to `.gitignore`
- Keep your API key secure and private

## Troubleshooting

### "Execution of scripts is disabled"
Run PowerShell as Administrator and execute:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### "Package already exists"
- Increment the version number in `ccxt.net.csproj`
- The script uses `--skip-duplicate` to avoid errors

### "Invalid API key"
- Verify your key at https://www.nuget.org/account/apikeys
- Ensure the key has "Push" permissions

### "Build failed"
- Run `dotnet build` manually to see detailed errors
- Fix any compilation errors before publishing

## Package Information

- **Package ID**: CCXT.NET
- **NuGet URL**: https://www.nuget.org/packages/CCXT.NET/
- **Source**: https://github.com/ccxt-net/ccxt.net

## Manual Publishing (Alternative)

If you prefer manual steps:

```bash
# 1. Build the project
dotnet build src/ccxt.net.csproj -c Release

# 2. Run tests
dotnet test tests/ccxt.tests.csproj -c Release

# 3. Create package
dotnet pack src/ccxt.net.csproj -c Release

# 4. Publish to NuGet
dotnet nuget push src/bin/Release/CCXT.NET.1.5.2.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

## Support

For issues or questions:
- GitHub Issues: https://github.com/ccxt-net/ccxt.net/issues
- Email: help@odinsoft.co.kr