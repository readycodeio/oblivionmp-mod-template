#!powershell.exe -ExecutionPolicy Bypass -File

# Renames the mod across the whole template: the three projects and their folders, the solution,
# namespaces and usings, the manifest's uniqueId, ModFiles.ps1 and the README.
#
#   .\RenameMod.ps1 Acme.TreasureHunt
#
# The current name is taken from the .sln file name, so this also works to rename again later.
# Pass -DryRun to see what would change without touching anything.

param (
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $NewName,

    [switch] $DryRun
)

$ErrorActionPreference = 'Stop'

$scriptDir = $PSScriptRoot

if ($NewName -notmatch '^[A-Za-z_][A-Za-z0-9_]*(\.[A-Za-z_][A-Za-z0-9_]*)*$')
{
    Write-Host "'$NewName' is not usable as a namespace and folder name."
    Write-Host "Use letters, digits and underscores, optionally dot-separated, e.g. Acme.TreasureHunt."
    Exit 1
}

$solutionFiles = Get-ChildItem -Path $scriptDir -Filter *.sln
if ($solutionFiles.Count -ne 1)
{
    Write-Host "Error: Expected exactly one .sln file in $scriptDir, found $( $solutionFiles.Count )."
    Exit 1
}

$oldName = [System.IO.Path]::GetFileNameWithoutExtension($solutionFiles[0].Name)

if ($oldName -eq $NewName)
{
    Write-Host "Already named $NewName, nothing to do."
    Exit 0
}

foreach ($suffix in 'Client', 'Common', 'Server')
{
    $target = Join-Path $scriptDir "$NewName.$suffix"
    if (Test-Path $target)
    {
        Write-Host "Error: $target already exists."
        Exit 1
    }
}

Write-Host "Renaming $oldName -> $NewName$( if ($DryRun) { ' (dry run)' } )"
Write-Host ""

# Stale artifacts still carry the old assembly names, so clear them out.
$junk = @(Join-Path $scriptDir 'Output'; Join-Path $scriptDir 'build.log')
$junk += Get-ChildItem $scriptDir -Recurse -Directory -Include bin, obj -ErrorAction SilentlyContinue |
        Select-Object -ExpandProperty FullName
foreach ($path in $junk)
{
    if (Test-Path $path)
    {
        if (-not $DryRun)
        {
            Remove-Item $path -Recurse -Force
        }
        Write-Host "  cleaned $( $path.Substring($scriptDir.Length + 1) )"
    }
}

# Replaces the name inside a text file, keeping whatever byte-order mark it already had.
function Update-FileText
{
    param(
        [Parameter(Mandatory = $true)][string] $Path,
        [Parameter(Mandatory = $true)][string] $From,
        [Parameter(Mandatory = $true)][string] $To,
        [Parameter(Mandatory = $true)][bool]   $Apply
    )

    $bytes = [System.IO.File]::ReadAllBytes($Path)
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $text = [System.Text.Encoding]::UTF8.GetString($bytes)
    if ($hasBom)
    {
        $text = $text.Substring(1)
    }

    $count = ([regex]::Matches($text, [regex]::Escape($From))).Count
    if ($count -gt 0 -and $Apply)
    {
        [System.IO.File]::WriteAllText($Path, $text.Replace($From, $To), (New-Object System.Text.UTF8Encoding($hasBom)))
    }

    return $count
}

# Dependencies holds the SDK assemblies, .git and .idea are not ours to rewrite, and the rest is
# build output that the cleanup above already removed.
$skipped = @('Dependencies', 'Output', 'bin', 'obj', '.git', '.idea')
$selfPath = $PSCommandPath
$isOurs = {
    $path = $_.FullName
    if ($path -eq $selfPath)
    {
        return $false
    }

    $segments = $path.Substring($scriptDir.Length + 1).Split([System.IO.Path]::DirectorySeparatorChar)
    -not ($segments | Where-Object { $_ -in $skipped })
}

$textFiles = Get-ChildItem $scriptDir -Recurse -File -Include *.cs, *.csproj, *.sln, *.json, *.ps1, *.md |
        Where-Object $isOurs

foreach ($file in $textFiles)
{
    $count = Update-FileText -Path $file.FullName -From $oldName -To $NewName -Apply (-not $DryRun)
    if ($count -gt 0)
    {
        Write-Host "  $count in $( $file.FullName.Substring($scriptDir.Length + 1) )"
    }
}

# Files first, then directories deepest-first, so no path goes stale mid-rename.
$toRename = @(Get-ChildItem $scriptDir -Recurse -File | Where-Object { $_.Name -like "*$oldName*" } | Where-Object $isOurs)
$toRename += @(Get-ChildItem $scriptDir -Recurse -Directory | Where-Object { $_.Name -like "*$oldName*" } | Where-Object $isOurs |
        Sort-Object { $_.FullName.Length } -Descending)

foreach ($item in $toRename)
{
    $newLeaf = $item.Name.Replace($oldName, $NewName)
    if (-not $DryRun)
    {
        Rename-Item -Path $item.FullName -NewName $newLeaf
    }
    Write-Host "  $( $item.Name ) -> $newLeaf"
}

Write-Host ""
if ($DryRun)
{
    Write-Host "Dry run, nothing was changed."
}
else
{
    Write-Host "Done. Open $NewName.sln and build it to check the rename."
    Write-Host "Class names like ExampleServerRpc are feature names, not the mod name, so rename those yourself."
}
