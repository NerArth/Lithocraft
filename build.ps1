param (
    [string]$Target = "",
    [string]$Config = "",
    [string]$VSPath = "",
    [switch]$Help
)

if ($Help) {
    Write-Host "Lithocraft Build Script"
    Write-Host "-----------------------"
    Write-Host "Usage: .\build.ps1 [-Target <Target>] [-Config <Configuration>] [-VSPath <Path>]"
    Write-Host ""
    Write-Host "Parameters:"
    Write-Host "  -Target   : The target module to build. Valid options: 'All', 'Core', 'Chem', 'Metal'."
    Write-Host "  -Config   : The build configuration. Valid options: 'Debug', 'Release'."
    Write-Host "  -VSPath   : Optional. The path to the Vintage Story installation directory."
    Write-Host "              Overrides the VINTAGE_STORY environment variable."
    Write-Host "  -Help     : Displays this help message."
    Write-Host ""
    Write-Host "If executed without parameters, the script will run in interactive mode."
    exit 0
}

$IsInteractive = ($Target -eq "" -and $Config -eq "" -and $VSPath -eq "")

if ($IsInteractive) {
    Write-Host "Lithocraft Interactive Build"
    Write-Host "----------------------------"

    # Target selection
    $validTargets = @("All", "Core", "Chem", "Metal")
    while ($true) {
        $TargetInput = Read-Host "Select target module to build (All, Core, Chem, Metal) [Default: All]"
        if ([string]::IsNullOrWhiteSpace($TargetInput)) {
            $Target = "All"
            break
        }
        if ($validTargets -contains $TargetInput) {
            $Target = $TargetInput
            break
        }
        Write-Host "Invalid target selected. Please try again." -ForegroundColor Red
    }

    # Configuration selection
    $validConfigs = @("Debug", "Release")
    while ($true) {
        $ConfigInput = Read-Host "Select configuration (Debug, Release) [Default: Debug]"
        if ([string]::IsNullOrWhiteSpace($ConfigInput)) {
            $Config = "Debug"
            break
        }
        if ($validConfigs -contains $ConfigInput) {
            $Config = $ConfigInput
            break
        }
        Write-Host "Invalid configuration selected. Please try again." -ForegroundColor Red
    }
} else {
    # Validate non-interactive inputs
    if ($Target -eq "") { $Target = "All" }
    if ($Config -eq "") { $Config = "Debug" }

    $validTargets = @("All", "Core", "Chem", "Metal")
    if ($validTargets -notcontains $Target) {
        Write-Host "Error: Invalid Target '$Target'." -ForegroundColor Red
        exit 1
    }

    $validConfigs = @("Debug", "Release")
    if ($validConfigs -notcontains $Config) {
        Write-Host "Error: Invalid Config '$Config'." -ForegroundColor Red
        exit 1
    }
}

# Resolve Vintage Story Path
$EnvVarVSPath = [Environment]::GetEnvironmentVariable("VINTAGE_STORY")
if ($VSPath -ne "") {
    $FinalVSPath = $VSPath
} elseif ($EnvVarVSPath -ne $null -and $EnvVarVSPath -ne "") {
    $FinalVSPath = $EnvVarVSPath
} else {
    if ($IsInteractive) {
        Write-Host "VINTAGE_STORY environment variable is not set." -ForegroundColor Yellow
        $FinalVSPath = Read-Host "Please provide the path to your Vintage Story installation"
        if ([string]::IsNullOrWhiteSpace($FinalVSPath) -or !(Test-Path $FinalVSPath)) {
            Write-Host "Error: A valid Vintage Story path is required to build." -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "Error: VINTAGE_STORY environment variable is not set and no -VSPath was provided." -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "Build Settings:" -ForegroundColor Cyan
Write-Host "  Target        : $Target"
Write-Host "  Configuration : $Config"
Write-Host "  Vintage Story : $FinalVSPath"
Write-Host ""

# Determine which project/solution to build
if ($Target -eq "All") {
    $ProjectToBuild = "Lithocraft.sln"
} else {
    $ProjectToBuild = "Modules\Lithocraft-$Target\Lithocraft-$Target.csproj"
}

if (!(Test-Path $ProjectToBuild)) {
    Write-Host "Error: Project or solution file '$ProjectToBuild' not found." -ForegroundColor Red
    exit 1
}

# Build Command
$BuildArgs = @(
    "build",
    $ProjectToBuild,
    "-c", $Config,
    "-p:VINTAGE_STORY=$FinalVSPath"
)

Write-Host "Executing: dotnet $BuildArgs" -ForegroundColor Cyan
dotnet $BuildArgs

$ExitCode = $LASTEXITCODE
if ($ExitCode -eq 0) {
    Write-Host "Build completed successfully." -ForegroundColor Green
} else {
    Write-Host "Build failed with exit code $ExitCode." -ForegroundColor Red
}

exit $ExitCode
