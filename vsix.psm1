
$VsixCs = Resolve-Path ".\*\Vsix.cs"
$VsixCsVersionPattern = 'VERSION = "([\d\\.]+)"'
$VsixCsVersionFormat = 'VERSION = "{0}"'

$SourceManifest = Resolve-Path ".\*\source.extension.vsixmanifest"
$PublishManifest = ".\publish.manifest.json"
$PublishVsixFile = Resolve-Path ".\*\bin\Release\*.vsix"


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

    [xml]$content = Get-Content $SourceManifest
    $content.PackageManifest.Metadata.Identity.Version = $Version
    $content.Save($SourceManifest)

    Write-Host "Version updated:" $SourceManifest -ForegroundColor Green    
}

function PublishVsix {
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)] 
        [string] $PAT
    )
    
    $installation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -format json | ConvertFrom-Json
    $path = $installation.installationPath
    $vsixPublisher = Join-Path -Path $path -ChildPath "VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe" -Resolve
    Write-Host "VsixPublisher:" $vsixPublisher -ForegroundColor Green
    
    Write-Host "Publish Manifest:" $PublishManifest -ForegroundColor Green
    Write-Host "Vsix:" $PublishVsixFile -ForegroundColor Green

    & $vsixPublisher publish -payload $PublishVsixFile -publishManifest $PublishManifest -personalAccessToken $PAT -ignoreWarnings "VSIXValidatorWarning01,VSIXValidatorWarning02,VSIXValidatorWarning08"
}

Export-ModuleMember -Function UpdateVersion, PublishVsix