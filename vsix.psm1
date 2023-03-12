
$CsVersionFile = "**\Package.cs"
$CsVersionPattern = 'VERSION = "([\d\\.]+)"'
$CsVersionFormat = 'VERSION = "{0}"'
$VsixManifests = "**\source.extension.vsixmanifest"

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

    $file = Resolve-Path $CsVersionFile
    (Get-Content $file) | ForEach-Object {
        if ($_ -cmatch $CsVersionPattern){
            $_ -creplace $CsVersionPattern, ($CsVersionFormat -f $Version)
        }
        else {
            $_
        }
    } | Set-Content $file -Encoding UTF8

    Write-Host "Version updated:" $file -ForegroundColor Green
}

function UpdateManifestVersion {
    param(
        [string] $Version
    )

    $files = Resolve-Path $VsixManifests
    $files | ForEach-Object {
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
    
    $vsix = Resolve-Path $vsix
    $manifest = Resolve-Path $manifest

    $installation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -format json | ConvertFrom-Json
    $path = $installation.installationPath
    $vsixPublisher = Join-Path -Path $path -ChildPath "VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe" -Resolve

    Write-Host "VsixPublisher:" $vsixPublisher -ForegroundColor Green
    Write-Host "Manifest:" $manifest -ForegroundColor Green
    Write-Host "Vsix:" $vsix -ForegroundColor Green

    & $vsixPublisher publish -payload "$vsix" -publishManifest "$manifest" -personalAccessToken $PAT
}

Export-ModuleMember -Function UpdateVersion, PublishVsix