[CmdletBinding()]
param()

$osName = $null

if ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::OSX)) {
    $osName = "osx"
}
elseif ([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Linux)) {
    $osName = "linux"
}
else {
    $PSCmdlet.ThrowTerminatingError(
        [System.Management.Automation.ErrorRecord]::new(
            [System.PlatformNotSupportedException]::new("Unsupported OS platform."),
            "UnsupportedOS",
            [System.Management.Automation.ErrorCategory]::NotImplemented,
            $null
        )
    )
}

$osArch = $null
if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq [System.Runtime.InteropServices.Architecture]::Arm64) {
    $osArch = "arm64"
}
elseif ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq [System.Runtime.InteropServices.Architecture]::X64) {
    $osArch = "x64"
}
else {
    $PSCmdlet.ThrowTerminatingError(
        [System.Management.Automation.ErrorRecord]::new(
            [System.PlatformNotSupportedException]::new("Unsupported OS architecture."),
            "UnsupportedArch",
            [System.Management.Automation.ErrorCategory]::NotImplemented,
            $null
        )
    )
}

$artifactPath = Join-Path -Path $PSScriptRoot -ChildPath "artifacts/publish/Configurator/release_$($osName)-$($osArch)/vscode-configurator"
$templatesArtifactPath = Join-Path -Path $PSScriptRoot -ChildPath "artifacts/publish/Configurator/release_$($osName)-$($osArch)/Templates"
$installPath = Join-Path -Path ([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile)) -ChildPath ".vscodeconfigurator/bin/"

if (!(Test-Path -Path $artifactPath)) {
    $PSCmdlet.ThrowTerminatingError(
        [System.Management.Automation.ErrorRecord]::new(
            [System.IO.FileNotFoundException]::new("Built executable not found at '$($artifactPath)'."),
            "InstallerNotFound",
            [System.Management.Automation.ErrorCategory]::ObjectNotFound,
            $artifactPath
        )
    )
}

if (!(Test-Path -Path $installPath)) {
    Write-Warning "Creating install directory at: $($installPath)"
    $null = New-Item -Path $installPath -ItemType "Directory" -Force
}

Write-Verbose "Copying 'vscode-configurator' to: $($installPath)"
Copy-Item -Path $artifactPath -Destination $installPath -Force -Verbose:$false

Write-Verbose "Copying templates to: $($installPath)"
Copy-Item -Path $templatesArtifactPath -Destination $installPath -Recurse -Force -Verbose:$false
