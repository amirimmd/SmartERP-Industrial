###############################################################################
# SmartERP WPF — Self-Contained Publish Script
#
# Produces a single portable EXE in:
#   SmartERP.Wpf\publish\win-x64\SmartERP.exe
#
# Requirements on the TARGET machine:
#   • Windows 8.1+ (current net8.0-windows build)
#     — OR Windows 7 SP1+ if you switch to net6.0-windows in the .csproj
#   • WebView2 Runtime (Evergreen for Win8.1+, Fixed-Version v86 for Win7)
#     The EXE itself is self-contained for .NET; WebView2 is a separate install.
#
# Run from: SmartERP.Wpf\ directory
###############################################################################

$ErrorActionPreference = "Stop"

$project   = "SmartERP.Wpf.csproj"
$rid       = "win-x64"          # Change to "win-x86" for 32-bit Windows 7
$outDir    = "publish\$rid"

Write-Host ""
Write-Host "=== SmartERP WPF Publish ===" -ForegroundColor Cyan
Write-Host "Runtime identifier : $rid"
Write-Host "Output directory   : $outDir"
Write-Host ""

dotnet publish $project `
    --configuration Release `
    --runtime $rid `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:PublishReadyToRun=false `
    -p:DebugType=None `
    -p:DebugSymbols=false `
    -p:EnableCompressionInSingleFile=true `
    --output $outDir

Write-Host ""
Write-Host "=== Publish complete ===" -ForegroundColor Cyan
Write-Host "Output: $(Resolve-Path $outDir)" -ForegroundColor White

# ─────────────────────────────────────────────────────────────────────────────
# WINDOWS 7 ONLY — WebView2 Fixed-Version setup
# Uncomment the block below after placing the WebView2 Fixed-Version runtime
# (v86.0.622.51) in a subfolder called "WebView2" next to the EXE.
# ─────────────────────────────────────────────────────────────────────────────
<#
$webview2Src = ".\WebView2_FixedVersion"   # your downloaded fixed-version folder
$webview2Dst = "$outDir\WebView2"

if (Test-Path $webview2Src) {
    Copy-Item -Path $webview2Src -Destination $webview2Dst -Recurse -Force
    Write-Host "WebView2 Fixed-Version runtime copied to output." -ForegroundColor Green
} else {
    Write-Warning "WebView2 fixed-version folder not found at: $webview2Src"
    Write-Warning "Windows 7 clients will need the WebView2 runtime installed separately."
}
#>
