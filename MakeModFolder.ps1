#!powershell.exe -ExecutionPolicy Bypass -File

param (
    [string] $Configuration
)

# Check params
if (-not $Configuration)
{
    Write-Host "Usage: .\MakeModFolder.ps1 <Debug|Release>"
    Exit 1
}

# Source the mod file lists
. ./ModFiles.ps1

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Find the single .sln file
$solutionFiles = Get-ChildItem -Path $scriptDir -Filter *.sln
if ($solutionFiles.Count -eq 1) {
    $solutionPath = $solutionFiles[0].FullName
} else {
    Write-Host "Error: Expected exactly one .sln file in $scriptDir, found $($solutionFiles.Count)."
    Exit 1
}

# Build the whole solution (all three projects)
Write-Output "Building solution $solutionPath in configuration $Configuration..."
dotnet build $solutionPath -c $Configuration -v minimal /t:Rebuild | Tee-Object -FilePath 'build.log'
if ($LASTEXITCODE -ne 0)
{
    Write-Error "Build failed. See build.log for details."
    Exit 1
}

# Reset the Output directory
$outputRoot = Join-Path $scriptDir 'Output'
if (-not (Test-Path $outputRoot))
{
    New-Item -ItemType Directory -Path $outputRoot -Force | Out-Null
} else {
    Get-ChildItem $outputRoot -Recurse | Remove-Item -Force -Recurse
}

$tfm = 'net10.0'

# The two mods and where their files come from. Each becomes its own folder under Output:
#   ExampleMod.Client -> copy into the server's mods/ directory (as a folder or a .zip)
#   ExampleMod.Server -> copy into the server's server_mods/ directory
$clientBuildDir = Join-Path $scriptDir "ExampleMod.Client/bin/$Configuration/$tfm"
$serverBuildDir = Join-Path $scriptDir "ExampleMod.Server/bin/$Configuration/$tfm"
$contentDir     = Join-Path $scriptDir 'Content'

# Copies the named files from $BaseDir into $DestRoot, preserving any relative subpaths.
function Copy-BuildArtifacts
{
    param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyCollection()]
        [string[]] $Files,

        [Parameter(Mandatory = $true)]
        [string] $BaseDir,

        [Parameter(Mandatory = $true)]
        [string] $DestRoot
    )

    foreach ($file in $Files)
    {
        $sourceFile = Join-Path -Path $BaseDir -ChildPath $file
        $destFile = Join-Path -Path $DestRoot -ChildPath $file

        if (Test-Path -Path $sourceFile)
        {
            $destDir = Split-Path -Parent $destFile
            if (-not (Test-Path -Path $destDir))
            {
                New-Item -ItemType Directory -Path $destDir -Force | Out-Null
            }

            Copy-Item -Path $sourceFile -Destination $destFile -Force
            Write-Output "Copied $file"
        }
        else
        {
            Write-Warning "Source file not found: $sourceFile"
        }
    }
}

# --- Client mod ---
$clientDest = Join-Path $outputRoot 'ExampleMod.Client'
New-Item -ItemType Directory -Path $clientDest -Force | Out-Null

$clientFiles = $clientBuildFiles
if ($Configuration -eq 'Debug')
{
    $clientFiles += $clientDebugFiles
}

Write-Output "`nPackaging client mod -> $clientDest"
Copy-BuildArtifacts -Files $clientFiles -BaseDir $clientBuildDir -DestRoot $clientDest
Copy-BuildArtifacts -Files $clientContentFiles -BaseDir $contentDir -DestRoot $clientDest

# --- Server mod ---
$serverDest = Join-Path $outputRoot 'ExampleMod.Server'
New-Item -ItemType Directory -Path $serverDest -Force | Out-Null

$serverFiles = $serverBuildFiles
if ($Configuration -eq 'Debug')
{
    $serverFiles += $serverDebugFiles
}

Write-Output "`nPackaging server mod -> $serverDest"
Copy-BuildArtifacts -Files $serverFiles -BaseDir $serverBuildDir -DestRoot $serverDest

# Open explorer to the output directory
if ($PSVersionTable.PSEdition -eq 'Core')
{
    Start-Process "explorer.exe" -ArgumentList $outputRoot
}
else
{
    Invoke-Item $outputRoot
}
