# VdO Project - Automated Build Fix Script for Windows
# Run this script as Administrator in PowerShell

param(
    [string]$ProjectPath = ".",
    [switch]$RemoveCodeSigning = $false,
    [switch]$CreateStubLibraries = $false
)

Write-Host "====================================" -ForegroundColor Cyan
Write-Host "VdO Build Error Fix Script" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as Administrator
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "WARNING: This script should be run as Administrator for best results" -ForegroundColor Yellow
    Write-Host ""
}

Set-Location $ProjectPath

# Step 1: Unblock all files
Write-Host "[1/5] Unblocking all files downloaded from internet..." -ForegroundColor Yellow
try {
    $files = Get-ChildItem -Path . -Recurse -File
    $blockedCount = 0
    foreach ($file in $files) {
        try {
            Unblock-File -Path $file.FullName -ErrorAction SilentlyContinue
            $blockedCount++
        } catch {
            # File might not be blocked, ignore
        }
    }
    Write-Host "✓ Unblocked files in project" -ForegroundColor Green
} catch {
    Write-Host "✗ Error unblocking files: $_" -ForegroundColor Red
}

# Step 2: Check for NuGet
Write-Host ""
Write-Host "[2/5] Checking NuGet installation..." -ForegroundColor Yellow
$nugetPath = Get-Command nuget -ErrorAction SilentlyContinue
if (-not $nugetPath) {
    Write-Host "NuGet CLI not found. Downloading..." -ForegroundColor Yellow
    $nugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    Invoke-WebRequest -Uri $nugetUrl -OutFile "nuget.exe"
    $env:Path += ";$PWD"
    Write-Host "✓ NuGet downloaded" -ForegroundColor Green
} else {
    Write-Host "✓ NuGet found at: $($nugetPath.Source)" -ForegroundColor Green
}

# Step 3: Enable NuGet Package Restore
Write-Host ""
Write-Host "[3/5] Enabling NuGet Package Restore..." -ForegroundColor Yellow
if (-not (Test-Path ".nuget")) {
    New-Item -ItemType Directory -Path ".nuget" -Force | Out-Null
}

# Create NuGet.config if it doesn't exist
$nugetConfig = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
  <packageRestore>
    <add key="enabled" value="True" />
    <add key="automatic" value="True" />
  </packageRestore>
</configuration>
"@

if (-not (Test-Path "NuGet.config")) {
    $nugetConfig | Out-File -FilePath "NuGet.config" -Encoding UTF8
    Write-Host "✓ Created NuGet.config" -ForegroundColor Green
}

# Step 4: Restore NuGet Packages
Write-Host ""
Write-Host "[4/5] Restoring NuGet packages..." -ForegroundColor Yellow
$solutionFiles = Get-ChildItem -Filter "*.sln"
foreach ($sln in $solutionFiles) {
    Write-Host "  Restoring packages for: $($sln.Name)" -ForegroundColor Gray
    & nuget restore $sln.FullName -NonInteractive
}
Write-Host "✓ Package restore completed" -ForegroundColor Green

# Step 5: Handle Code Signing (if requested)
if ($RemoveCodeSigning) {
    Write-Host ""
    Write-Host "[5/5] Removing code signing requirements..." -ForegroundColor Yellow
    $projectFiles = Get-ChildItem -Path . -Filter "*.csproj" -Recurse
    $modifiedCount = 0
    
    foreach ($proj in $projectFiles) {
        $content = Get-Content $proj.FullName -Raw
        $originalContent = $content
        
        # Remove signing configurations
        $content = $content -replace '<SignAssembly>true</SignAssembly>', '<SignAssembly>false</SignAssembly>'
        $content = $content -replace '<AssemblyOriginatorKeyFile>.*?</AssemblyOriginatorKeyFile>', ''
        $content = $content -replace '<DelaySign>.*?</DelaySign>', ''
        
        if ($content -ne $originalContent) {
            Set-Content -Path $proj.FullName -Value $content
            $modifiedCount++
            Write-Host "  Modified: $($proj.Name)" -ForegroundColor Gray
        }
    }
    Write-Host "✓ Removed code signing from $modifiedCount project(s)" -ForegroundColor Green
}

# Step 6: Create MP Library Stubs (if requested)
if ($CreateStubLibraries) {
    Write-Host ""
    Write-Host "[Bonus] Creating MP library stubs..." -ForegroundColor Yellow
    
    $stubProjectDir = "MPLibraryStubs"
    if (-not (Test-Path $stubProjectDir)) {
        New-Item -ItemType Directory -Path $stubProjectDir -Force | Out-Null
    }
    
    # Create stub files
    $mpLogHelper = @"
using System;

namespace MP
{
    public static class MPLogHelper
    {
        public static void Log(string message) => Console.WriteLine($"[LOG] {message}");
        public static void LogError(string message) => Console.WriteLine($"[ERROR] {message}");
        public static void LogWarning(string message) => Console.WriteLine($"[WARN] {message}");
        public static void LogDebug(string message) => Console.WriteLine($"[DEBUG] {message}");
    }
}
"@

    $mpUtils = @"
using System;
using System.IO;

namespace MP
{
    public static class MPUtils
    {
        public static string GetAppPath() => AppDomain.CurrentDomain.BaseDirectory;
        public static string GetTempPath() => Path.GetTempPath();
        public static bool IsNullOrEmpty(string value) => string.IsNullOrEmpty(value);
    }
}
"@

    $mpExtensions = @"
using System;

namespace MP
{
    public static class MPExtensionMethods
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        public static string SafeTrim(this string value) => value?.Trim() ?? string.Empty;
    }
}
"@

    $mpLogHelper | Out-File -FilePath "$stubProjectDir\MPLogHelper.cs" -Encoding UTF8
    $mpUtils | Out-File -FilePath "$stubProjectDir\MPUtils.cs" -Encoding UTF8
    $mpExtensions | Out-File -FilePath "$stubProjectDir\MPExtensionMethods.cs" -Encoding UTF8
    
    Write-Host "✓ Created MP library stub files in $stubProjectDir" -ForegroundColor Green
    Write-Host "  Add these files to a new Class Library project to resolve MP references" -ForegroundColor Gray
}

# Summary
Write-Host ""
Write-Host "====================================" -ForegroundColor Cyan
Write-Host "Build Fix Summary" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host "✓ Files unblocked" -ForegroundColor Green
Write-Host "✓ NuGet package restore enabled" -ForegroundColor Green
Write-Host "✓ Packages restored" -ForegroundColor Green

if ($RemoveCodeSigning) {
    Write-Host "✓ Code signing removed" -ForegroundColor Green
}

if ($CreateStubLibraries) {
    Write-Host "✓ MP library stubs created" -ForegroundColor Green
}

Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Open the solution in Visual Studio" -ForegroundColor White
Write-Host "2. If MP library errors persist, either:" -ForegroundColor White
Write-Host "   - Add the actual MP library DLLs as references" -ForegroundColor Gray
Write-Host "   - Or use the stub files in MPLibraryStubs folder" -ForegroundColor Gray
Write-Host "3. Build the solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host ""
Write-Host "If you still have errors, run with additional options:" -ForegroundColor Yellow
Write-Host "  .\fix-build-errors.ps1 -RemoveCodeSigning -CreateStubLibraries" -ForegroundColor Gray