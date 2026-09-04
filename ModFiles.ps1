#!powershell.exe -ExecutionPolicy Bypass -File

# Edit these lists to match your mod. MakeModFolder.ps1 uses them to build
#
#   Output/mods/<modName>/manifest.json
#   Output/mods/<modName>/client/...
#   Output/mods/<modName>/server/...
#
# The server hands the client folder to connecting players and keeps the server folder to itself.

# Folder name of the packaged mod. Conventionally the uniqueId from manifest.json.
$modName = "ExampleMod"

# The projects the two halves build from.
$clientProject = "ExampleMod.Client"
$serverProject = "ExampleMod.Server"

# --- Client half, from <clientProject>/bin/<Configuration>/net10.0 ---
$clientBuildFiles = @(
    "ExampleMod.Client.dll",
    "ExampleMod.Common.dll"   # shared assembly both halves need
)

# --- Server half, from <serverProject>/bin/<Configuration>/net10.0 ---
$serverBuildFiles = @(
    "ExampleMod.Server.dll",
    "ExampleMod.Common.dll"
)

# Copied from "Content" into the mod root, alongside the two halves.
$manifestFiles = @(
    "manifest.json"
)

# Copied from "Content" into client/ and server/. Add any non-code files your mod ships.
$clientContentFiles = @(
    # "icon.png"
)

$serverContentFiles = @(
    # "config.json"
)

# Added only in Debug builds.
$clientDebugBuildFiles = @(
    "ExampleMod.Client.pdb",
    "ExampleMod.Common.pdb"
)

$serverDebugBuildFiles = @(
    "ExampleMod.Server.pdb",
    "ExampleMod.Common.pdb"
)
