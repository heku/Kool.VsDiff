
$VsixCs = Resolve-Path ".\*\Vsix.cs"
$VsixCsVersionPattern = 'VERSION = "([\d\\.]+)"'
$VsixCsVersionFormat = 'VERSION = "{0}"'

$SourceManifests = Resolve-Path ".\*\source.extension.vsixmanifest"

function UpdateVersion {
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string] $Version
    )

    Write-Host "Update vsix version to :" $Version -ForegroundColor Green
    UpdateVersionFile $Version
    UpdateManifestVersion $Version
}

function UpdateVersionFile {
    param(
        [string] $Version
    )
    
    (Get-Content $VsixCs) | ForEach-Object {
        if ($_ -cmatch $VsixCsVersionPattern){
            $_ -creplace $VsixCsVersionPattern, ($VsixCsVersionFormat -f $Version)
        }
        else {
            $_
        }
    } | Set-Content $VsixCs -Encoding UTF8

    Write-Host "Version updated:" $VsixCs -ForegroundColor Green
}

function UpdateManifestVersion {
    param(
        [string] $Version
    )

    $SourceManifests | ForEach-Object {
        [xml]$content = Get-Content $_
        $content.PackageManifest.Metadata.Identity.Version = $Version
        $content.Save($_)

        Write-Host "Version updated:" $_ -ForegroundColor Green   
    } 
}

function PublishVsix {
    [cmdletbinding()]
    param(
        [string] $PAT,
        [string] $vsix,
        [string] $manifest
    )
    
    $installation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -format json | ConvertFrom-Json
    $path = $installation.installationPath
    $vsixPublisher = Join-Path -Path $path -ChildPath "VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe" -Resolve
    Write-Host "VsixPublisher:" $vsixPublisher -ForegroundColor Green
    
    Write-Host "Publish Manifest:" $manifest -ForegroundColor Green
    Write-Host "Vsix:" $vsix -ForegroundColor Green

    & $vsixPublisher publish -payload $vsix -publishManifest $manifest -personalAccessToken $PAT
}

Export-ModuleMember -Function UpdateVersion, PublishVsix