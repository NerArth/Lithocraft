# extract-version.ps1
$modinfo = Get-Content "modinfo.json" | Out-String | ConvertFrom-Json
$version = $modinfo.Version
@"
<Project>
  <PropertyGroup>
    <Version>$version</Version>
  </PropertyGroup>
</Project>
"@ | Set-Content "Version.props"