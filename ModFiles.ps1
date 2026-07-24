#!powershell.exe -ExecutionPolicy Bypass -File

# Edit these lists to specify which files end up in each packaged mod folder.
# The client mod goes in the server's "mods/" directory; the server mod in "server_mods/".

# --- Client mod (from ExampleMod.Client/bin/<Configuration>/net10.0) ---
$clientBuildFiles = @(
    "ExampleMod.Client.dll",
    "ExampleMod.Common.dll"   # shared assembly the mod depends on
)

# Copied from the "Content" folder to the client mod root
$clientContentFiles = @(
    # Add any non-code files your client mod uses here.
    "manifest.json"
)

# Copied only in Debug builds
$clientDebugFiles = @(
    "ExampleMod.Client.pdb",
    "ExampleMod.Common.pdb"
)

# --- Server mod (from ExampleMod.Server/bin/<Configuration>/net10.0) ---
$serverBuildFiles = @(
    "ExampleMod.Server.dll",
    "ExampleMod.Common.dll"   # shared assembly the mod depends on
)

# Copied only in Debug builds
$serverDebugFiles = @(
    "ExampleMod.Server.pdb",
    "ExampleMod.Common.pdb"
)
