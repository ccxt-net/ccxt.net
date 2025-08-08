# CCXT.NET NuGet Package Unlist Script
# This script unlists (hides) a specific version of the package from NuGet.org

param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [string]$ApiKey = "",
    
    [Parameter(Mandatory=$false)]
    [switch]$Force = $false
)

# Configuration
$PackageId = "CCXT.NET"
$NuGetSource = "https://api.nuget.org/v3/index.json"

# Color output functions
function Write-Success { param($Message) Write-Host $Message -ForegroundColor Green }
function Write-Info { param($Message) Write-Host $Message -ForegroundColor Cyan }
function Write-Warning { param($Message) Write-Host $Message -ForegroundColor Yellow }
function Write-Error { param($Message) Write-Host $Message -ForegroundColor Red }

# Banner
Write-Host ""
Write-Warning "========================================="
Write-Warning "  CCXT.NET NuGet Package Unlist Tool"
Write-Warning "========================================="
Write-Host ""

# Check if API key is provided or exists in environment
if ([string]::IsNullOrEmpty($ApiKey)) {
    $ApiKey = $env:NUGET_API_KEY
    if ([string]::IsNullOrEmpty($ApiKey)) {
        Write-Error "Error: NuGet API key not provided!"
        Write-Host ""
        Write-Host "Please provide API key using one of these methods:"
        Write-Host "  1. Pass as parameter: .\unlist-nuget.ps1 -Version 1.1.7 -ApiKey YOUR_KEY"
        Write-Host "  2. Set environment variable: `$env:NUGET_API_KEY = 'YOUR_KEY'"
        Write-Host ""
        Write-Host "Get your API key from: https://www.nuget.org/account/apikeys"
        exit 1
    } else {
        Write-Info "Using API key from environment variable NUGET_API_KEY"
    }
}

# Check if dotnet CLI is available
try {
    $dotnetVersion = dotnet --version
    Write-Info "Found .NET SDK version: $dotnetVersion"
} catch {
    Write-Error "Error: .NET SDK not found! Please install from https://dotnet.microsoft.com/download"
    exit 1
}

# Display package information
Write-Host ""
Write-Warning "About to UNLIST the following package:"
Write-Host "  Package ID: $PackageId"
Write-Host "  Version: $Version"
Write-Host ""
Write-Warning "NOTE: This will hide the package from search results but NOT delete it."
Write-Warning "Users with direct links or existing projects can still access it."
Write-Host ""

# Confirmation
if (-not $Force) {
    $confirmation = Read-Host "Are you sure you want to unlist this package? (y/N)"
    if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
        Write-Warning "Operation cancelled by user"
        exit 0
    }
}

# Unlist the package
Write-Host ""
Write-Info "Unlisting package from NuGet.org..."

$deleteCommand = "dotnet nuget delete `"$PackageId`" `"$Version`" --source `"$NuGetSource`" --api-key `"$ApiKey`" --non-interactive"
Write-Host "  Command: dotnet nuget delete $PackageId $Version --source $NuGetSource --api-key [HIDDEN] --non-interactive"

$deleteResult = Invoke-Expression $deleteCommand
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to unlist package!"
    Write-Host ""
    Write-Host "Common issues:"
    Write-Host "  - Invalid API key"
    Write-Host "  - Package version doesn't exist"
    Write-Host "  - Insufficient permissions"
    Write-Host "  - Package was already unlisted"
    exit 1
}

Write-Host ""
Write-Success "========================================="
Write-Success "  Package unlisted successfully!"
Write-Success "========================================="
Write-Host ""
Write-Info "Package: $PackageId version $Version"
Write-Info "Status: Hidden from NuGet.org search results"
Write-Info "Direct URL still accessible: https://www.nuget.org/packages/$PackageId/$Version"
Write-Host ""
Write-Warning "To completely remove a package, you must contact NuGet support."

Write-Host ""
Write-Success "Process completed!"