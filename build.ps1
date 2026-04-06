param (
    [string]$Target = "",
    [string]$Config = "",
    [string]$GameVersion = "",
    [string]$VSPath = "",
    [switch]$Help
)

if ($Help) {
    Write-Host "Lithocraft Build Script"
    Write-Host "-----------------------"
    Write-Host "Usage: .\build.ps1 [-Target <Target>] [-Config <Configuration>] [-GameVersion <Version>] [-VSPath <Path>]"
    Write-Host ""
    Write-Host "Parameters:"
    Write-Host "  -Target      : The target module to build. Valid options: 'All', 'Core', 'Chem', 'Metal'."
    Write-Host "  -Config      : The build configuration. Valid options: 'Debug', 'Release'."
    Write-Host "  -GameVersion : The target game version (e.g., '1.20.x', '1.21.x'). Uses corresponding env var (e.g., VINTAGE_STORY_1_20)."
    Write-Host "  -VSPath      : Optional. The explicit path to the Vintage Story installation directory."
    Write-Host "                 Overrides GameVersion and environment variables."
    Write-Host "  -Help        : Displays this help message."
    Write-Host ""
    Write-Host "If executed without parameters, the script will run in interactive mode."
    exit 0
}

$IsInteractive = ($Target -eq "" -and $Config -eq "" -and $GameVersion -eq "" -and $VSPath -eq "")

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

    # Game Version selection
    while ($true) {
        $VersionInput = Read-Host "Select game version target (e.g., 1.20.x, 1.21.x) [Default: generic VINTAGE_STORY env var]"
        if ([string]::IsNullOrWhiteSpace($VersionInput)) {
            $GameVersion = ""
            break
        }
        # basic validation
        if ($VersionInput -match "^\d+\.\d+(\.\w+)?$") {
            $GameVersion = $VersionInput
            break
        }
        Write-Host "Please enter a valid version format (like '1.20.x' or '1.21')." -ForegroundColor Red
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
$FinalVSPath = ""

if ($VSPath -ne "") {
    $FinalVSPath = $VSPath
} elseif ($GameVersion -ne "") {
    # Derive Env Var name from version, e.g., 1.20.x -> VINTAGE_STORY_1_20
    $EnvVarSuffix = $GameVersion -replace "\.x$", "" -replace "\.", "_"
    $EnvVarName = "VINTAGE_STORY_$EnvVarSuffix"

    $VersionedPath = [Environment]::GetEnvironmentVariable($EnvVarName, "User")
    if ([string]::IsNullOrEmpty($VersionedPath)) {
        $VersionedPath = [Environment]::GetEnvironmentVariable($EnvVarName, "Machine")
    }

    if (![string]::IsNullOrEmpty($VersionedPath)) {
        $FinalVSPath = $VersionedPath
    } else {
        if ($IsInteractive) {
            Write-Host "Environment variable '$EnvVarName' is not set." -ForegroundColor Yellow
            $NewPath = Read-Host "Please provide the absolute path to your Vintage Story $GameVersion installation"
            if ([string]::IsNullOrWhiteSpace($NewPath) -or !(Test-Path $NewPath)) {
                Write-Host "Error: A valid path is required." -ForegroundColor Red
                exit 1
            }
            # Set the environment variable for future use
            [Environment]::SetEnvironmentVariable($EnvVarName, $NewPath, "User")
            Write-Host "Saved '$NewPath' to user environment variable '$EnvVarName'." -ForegroundColor Green
            $FinalVSPath = $NewPath
        } else {
            Write-Host "Error: Target game version '$GameVersion' specified, but environment variable '$EnvVarName' is not set." -ForegroundColor Red
            exit 1
        }
    }
}

# Fallback to generic VINTAGE_STORY if specific version not provided/resolved
if ([string]::IsNullOrEmpty($FinalVSPath)) {
    $GenericPath = [Environment]::GetEnvironmentVariable("VINTAGE_STORY", "User")
    if ([string]::IsNullOrEmpty($GenericPath)) {
        $GenericPath = [Environment]::GetEnvironmentVariable("VINTAGE_STORY", "Machine")
    }

    if (![string]::IsNullOrEmpty($GenericPath)) {
        $FinalVSPath = $GenericPath
    } else {
        if ($IsInteractive) {
            Write-Host "Generic 'VINTAGE_STORY' environment variable is not set." -ForegroundColor Yellow
            $NewPath = Read-Host "Please provide the absolute path to your default Vintage Story installation"
            if ([string]::IsNullOrWhiteSpace($NewPath) -or !(Test-Path $NewPath)) {
                Write-Host "Error: A valid path is required." -ForegroundColor Red
                exit 1
            }
            [Environment]::SetEnvironmentVariable("VINTAGE_STORY", $NewPath, "User")
            Write-Host "Saved '$NewPath' to user environment variable 'VINTAGE_STORY'." -ForegroundColor Green
            $FinalVSPath = $NewPath
        } else {
            Write-Host "Error: No valid Vintage Story path resolved. Ensure environment variables are set." -ForegroundColor Red
            exit 1
        }
    }
}

Write-Host ""
Write-Host "Build Settings:" -ForegroundColor Cyan
Write-Host "  Target        : $Target"
Write-Host "  Configuration : $Config"
if ($GameVersion -ne "") {
    Write-Host "  Game Version  : $GameVersion"
}
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
