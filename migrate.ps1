###############################################################################
# SmartERP MAUI → WPF Migration Script
# Run from: c:\Users\Mostafa\SmartERP\
# Prerequisite: Close Visual Studio / Rider before running.
###############################################################################

$ErrorActionPreference = "Stop"

$root = "C:\Users\Mostafa\SmartERP"
$wpf  = "C:\Users\Mostafa\SmartERP\SmartERP.Wpf"

Write-Host ""
Write-Host "=== SmartERP MAUI → WPF Migration ===" -ForegroundColor Cyan
Write-Host ""

# ─────────────────────────────────────────────────────────────────────────────
# STEP 1 — Move the five shared-code folders (Core, UI, Infrastructure,
#           Helpers, Modules) into the WPF project.
#           These contain no MAUI dependencies; they copy as-is.
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[1/6] Moving shared source folders..." -ForegroundColor Yellow

foreach ($folder in @("Core", "UI", "Infrastructure", "Helpers", "Modules")) {
    if (Test-Path "$root\$folder") {
        Copy-Item -Path "$root\$folder" -Destination "$wpf\$folder" -Recurse -Force
        Remove-Item -Path "$root\$folder" -Recurse -Force
        Write-Host "      OK  $folder" -ForegroundColor Green
    } else {
        Write-Host "      SKIP $folder (not found at root)" -ForegroundColor DarkYellow
    }
}

# ─────────────────────────────────────────────────────────────────────────────
# STEP 2 — Move wwwroot static assets.
#           The WPF project already has a correct wwwroot\index.html — we must
#           NOT overwrite it.  We copy everything else (css, favicon, etc.).
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[2/6] Moving wwwroot (preserving WPF index.html)..." -ForegroundColor Yellow

if (Test-Path "$root\wwwroot") {
    Get-ChildItem -Path "$root\wwwroot" -Exclude "index.html" | ForEach-Object {
        Copy-Item -Path $_.FullName -Destination "$wpf\wwwroot\" -Recurse -Force
        Write-Host "      Copied  wwwroot\$($_.Name)" -ForegroundColor Green
    }
    Remove-Item -Path "$root\wwwroot" -Recurse -Force
    Write-Host "      OK  wwwroot (index.html preserved)" -ForegroundColor Green
}

# ─────────────────────────────────────────────────────────────────────────────
# STEP 3 — Move Components folder contents with two exclusions:
#   • App.razor   — full-HTML MAUI host document; has no role in WPF
#   • Routes.razor — the WPF-fixed version already lives in SmartERP.Wpf\Components\
#                    and must not be overwritten with the MAUI version
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[3/6] Moving Components (skipping App.razor, keeping WPF Routes.razor)..." -ForegroundColor Yellow

$skipFiles = @("App.razor", "Routes.razor")

if (Test-Path "$root\Components") {
    Get-ChildItem -Path "$root\Components" -Recurse | ForEach-Object {
        if ($_.Name -notin $skipFiles) {
            $relative = $_.FullName.Substring("$root\Components\".Length)
            $target   = "$wpf\Components\$relative"

            if ($_.PSIsContainer) {
                New-Item -ItemType Directory -Force -Path $target | Out-Null
            } else {
                # Ensure parent directory exists before copy
                $parentDir = Split-Path -Parent $target
                if (-not (Test-Path $parentDir)) {
                    New-Item -ItemType Directory -Force -Path $parentDir | Out-Null
                }
                Copy-Item -Path $_.FullName -Destination $target -Force
                Write-Host "      Copied  Components\$relative" -ForegroundColor Green
            }
        } else {
            Write-Host "      SKIP   Components\$($_.Name)" -ForegroundColor DarkYellow
        }
    }
    Remove-Item -Path "$root\Components" -Recurse -Force
    Write-Host "      OK  Components" -ForegroundColor Green
}

# ─────────────────────────────────────────────────────────────────────────────
# STEP 4 — Delete obsolete MAUI files from the root directory.
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[4/6] Deleting MAUI-specific files..." -ForegroundColor Yellow

$mauiFiles = @(
    "MauiProgram.cs",
    "MainPage.xaml",
    "MainPage.xaml.cs",
    "App.xaml",
    "App.xaml.cs",
    "SmartERP.csproj",
    "SmartERP.csproj.user"
)

foreach ($f in $mauiFiles) {
    if (Test-Path "$root\$f") {
        Remove-Item -Path "$root\$f" -Force
        Write-Host "      Deleted  $f" -ForegroundColor Green
    }
}

$mauiFolders = @("Platforms", "Resources", "Properties", "bin", "obj")

foreach ($d in $mauiFolders) {
    if (Test-Path "$root\$d") {
        Remove-Item -Path "$root\$d" -Recurse -Force
        Write-Host "      Deleted  $d\" -ForegroundColor Green
    }
}

# ─────────────────────────────────────────────────────────────────────────────
# STEP 5 — Remove the old MAUI solution file and create a clean one that
#           points only to the new WPF project.
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[5/6] Recreating solution file..." -ForegroundColor Yellow

if (Test-Path "$root\SmartERP.sln") {
    Remove-Item -Path "$root\SmartERP.sln" -Force
    Write-Host "      Deleted  SmartERP.sln" -ForegroundColor Green
}

Push-Location $root
dotnet new sln --name SmartERP --output . --force 2>&1 | Out-Null
dotnet sln SmartERP.sln add SmartERP.Wpf\SmartERP.Wpf.csproj 2>&1 | Out-Null
Pop-Location

Write-Host "      OK  SmartERP.sln → SmartERP.Wpf\SmartERP.Wpf.csproj" -ForegroundColor Green

# ─────────────────────────────────────────────────────────────────────────────
# STEP 6 — Remove the empty Resources folder that was created inside SmartERP.Wpf
#           by the previous scaffold (it had no content).
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "[6/6] Cleaning up empty scaffold artifacts..." -ForegroundColor Yellow

if (Test-Path "$wpf\Resources") {
    $items = Get-ChildItem "$wpf\Resources" -Recurse
    if ($items.Count -eq 0) {
        Remove-Item "$wpf\Resources" -Recurse -Force
        Write-Host "      Deleted  SmartERP.Wpf\Resources\ (was empty)" -ForegroundColor Green
    }
}

# ─────────────────────────────────────────────────────────────────────────────
# FINAL — Verify the WPF project restores and builds cleanly
# ─────────────────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "=== Running dotnet restore + build ===" -ForegroundColor Cyan

Push-Location "$wpf"
dotnet restore
dotnet build --no-restore -c Debug
Pop-Location

Write-Host ""
Write-Host "=== Migration complete! ===" -ForegroundColor Cyan
Write-Host "Working directory: $wpf" -ForegroundColor White
Write-Host ""
