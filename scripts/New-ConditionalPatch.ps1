<#
.SYNOPSIS
Creates a new Vintage Story conditional JSON patch file for the Lithocraft ecosystem.

.DESCRIPTION
This script generates a boilerplate JSON patch file configured with a `dependsOn` condition.
This is useful for creating data fallbacks or expansions when optional modules (like Chem or Metal) are installed.

.PARAMETER ModulePath
The root path of the module where the patch should be created (e.g., "Modules/Lithocraft-Metal").

.PARAMETER TargetFile
The Vintage Story domain path of the file being patched (e.g., "lithocraftcore:recipes/grid/smelting_basic.json").

.PARAMETER DependsOnModId
The Mod ID that must be installed for this patch to apply (e.g., "lithocraftchem").

.PARAMETER OutputFileName
The name of the patch file to create (e.g., "chem_smelting_upgrade.json").

.EXAMPLE
.\scripts\New-ConditionalPatch.ps1 -ModulePath "Modules/Lithocraft-Metal" -TargetFile "lithocraftcore:recipes/grid/smelting_basic.json" -DependsOnModId "lithocraftchem" -OutputFileName "chem_smelting_upgrade.json"
#>

param (
    [Parameter(Mandatory=$true)]
    [string]$ModulePath,

    [Parameter(Mandatory=$true)]
    [string]$TargetFile,

    [Parameter(Mandatory=$true)]
    [string]$DependsOnModId,

    [Parameter(Mandatory=$true)]
    [string]$OutputFileName,

    [string]$PatchOp = "replace",
    [string]$PatchPath = "/ingredients/0",
    [string]$PatchValue = "{ `"type`": `"item`", `"code`": `"lithocraftchem:catalyst`" }"
)

$ErrorActionPreference = "Stop"

# Ensure the patches directory exists
# Assuming standard VS structure: assets/<modid>/patches/
# First we need to extract the modid from the module's modinfo.json
$modInfoPath = Join-Path $ModulePath "modinfo.json"
if (-not (Test-Path $modInfoPath)) {
    Write-Error "Could not find modinfo.json at $modInfoPath"
    exit 1
}

$modInfo = Get-Content $modInfoPath | ConvertFrom-Json
$modId = $modInfo.ModID

$patchesDir = Join-Path $ModulePath "assets/$modId/patches"
if (-not (Test-Path $patchesDir)) {
    Write-Host "Creating patches directory: $patchesDir"
    New-Item -ItemType Directory -Path $patchesDir -Force | Out-Null
}

$outputPath = Join-Path $patchesDir $OutputFileName

if (Test-Path $outputPath) {
    Write-Warning "File $outputPath already exists. Please delete it first or choose a different name."
    exit 1
}

# Construct the JSON string. Since $PatchValue is expected to be valid JSON, we won't wrap it in quotes here.
# Note: PowerShell's ConvertTo-Json can be tricky with nested pre-formatted JSON strings,
# so we build the template manually for the boilerplate to ensure exact formatting.
$jsonContent = @"
[
  {
    "file": "$TargetFile",
    "op": "$PatchOp",
    "path": "$PatchPath",
    "value": $PatchValue,
    "dependsOn": [{ "modid": "$DependsOnModId" }]
  }
]
"@

Set-Content -Path $outputPath -Value $jsonContent -Encoding UTF8

Write-Host "Success: Created conditional patch file at $outputPath"
Write-Host "Targeting: $TargetFile"
Write-Host "Depends on Mod: $DependsOnModId"
