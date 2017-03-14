$VsixName = "hub-visualstudio"
$Extension = ".vsix"

$BuildModule = Join-Path $PSScriptRoot "Invoke-MsBuild.psm1"
$Solution = Join-Path $PSScriptRoot "BlackDuckHubVS.sln"
$Release = Join-Path $PSScriptRoot "hub-visualstudio\bin\Release"
$Manifest = Join-Path $PSScriptRoot "hub-visualstudio\source.extension.vsixmanifest"
$Vsix = Join-Path $Release ("{0}{1}" -f $VsixName, $Extension)

if (Test-Path $Release) {
    Remove-Item ("{0}\*{1}" -f $Release, $Extension)
}

[xml]$XmlDocument = Get-Content -Path $Manifest

$Version = $XmlDocument.PackageManifest.Metadata.Identity.Version

Import-Module -Name $BuildModule
$buildResult = Invoke-MsBuild -Path $Solution -Params "/target:Clean;Build /property:Configuration=Release;Platform=""Any CPU"";BuildInParallel=true;DeployExtension=false" -Use32BitMsBuild

if ($buildResult.BuildSucceeded -eq $true) { 
    Rename-Item -Path $Vsix -NewName ("{0}-{1}{2}" -f $VsixName, $Version, $Extension)
    Write-Host "VSIX created successfully." 
}
elseif (!$buildResult.BuildSucceeded -eq $false) { 
    Write-Host "VSIX creation failed. Check the build log file $($buildResult.BuildLogFilePath) for errors."
}