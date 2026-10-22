# OblivionMP Mod Template

![version](https://img.shields.io/badge/version-1.0.0--rc.3-green)

For other versions, check the list of [tags](https://github.com/readycodeio/oblivionmp-mod-template/tags).

A template for developing an OblivionMP mod with the OblivionMP SDK.

Refer to the [OblivionMP SDK documentation](https://docs.ready.mp) for details on how to use the SDK.

## Requirements

* [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) or later
* If you are using Visual Studio, you need version 2026 or later

## Repository structure

An OblivionMP mod is usually made of two halves that share a common core:

- `ExampleMod.Common/`: shared by both halves. The `Wallet` mixin (`Wallet.cs`) and every message the
  mod sends (`RpcContracts.cs`) live here. Its DLL ships alongside both halves.
- `ExampleMod.Client/`: the client half, loaded by the game. Entry point (`Mod.cs`), the client end of
  the messages (`RpcHandlers.cs`) and a service that binds keys (`WalletKeys.cs`).
- `ExampleMod.Server/`: the server half, loaded by the relay server. Entry point (`Mod.cs`), the
  server end of the messages (`RpcHandlers.cs`) and a service that runs every tick
  (`PassiveIncome.cs`).
- `Content/manifest.json`: mod metadata (id, name, version, dependencies).

The projects compile against the OblivionMP SDK packages from NuGet, one per half:
`ReadyM.SDK.Oblivion.Common` for the shared project, `ReadyM.SDK.Oblivion.Client` for the game and
`ReadyM.SDK.Oblivion.Server` for the relay. The game and the relay provide those assemblies at
runtime, so they never end up in your mod folder.

The example is a small wallet feature: a `Balance` on every player that the server owns and every
client can read, a request to add gold and one to read the balance, a server service that grants
passive income, and a ping one player sends to the others nearby. Use it as a starting point and
replace it with your own logic.

Key bindings in game: Alt+N adds gold, Alt+B shows your balance, Alt+P pings nearby players. The
results go to the client log.

## Getting started

1. Clone this repository to your local machine.
2. Open `ExampleMod.sln` in your preferred C# IDE (e.g. JetBrains Rider, Visual Studio) and build it.
   The SDK packages restore from NuGet.
3. Rename the mod so it does not collide with other mods on a server:

   ```powershell
   .\RenameMod.ps1 Acme.TreasureHunt
   ```

4. Edit the code to build your own mod. Keep mixins and RPC contracts in the Common project so both
   halves agree on them.

> **Note**: both halves have to agree on every mixin and every message. Ship them together as one
> mod folder, which is what the packaging script produces.

## Packaging the mod

1. Edit `Content/manifest.json` with your mod's id, name, version and description. `uniqueId` has to
   be unique across every mod on the server, and dependency ids are matched exactly, case included.
2. Edit `ModFiles.ps1` if your mod ships extra files or you renamed the projects.
3. Run the packaging script with the `Release` argument:

   ```powershell
   .\MakeModFolder.ps1 Release
   ```

4. `Output/mods/` will contain one folder for your mod:

   ```
   mods/ExampleMod/manifest.json
   mods/ExampleMod/client/     <- handed out to connecting players
   mods/ExampleMod/server/     <- stays on the server
   ```

5. Copy that folder into your server's `mods/` directory and restart the server. Connecting clients
   download the client half automatically; the server half never leaves the server.

Pass `-NoExplorer` to skip opening the output folder, for example when calling the script from CI.

## Debugging

Run the packaging script with the `Debug` argument to include `.pdb` symbol files:

```powershell
.\MakeModFolder.ps1 Debug
```

If you downloaded this template as a ZIP rather than cloning it, Windows marks the scripts as remote
and PowerShell refuses to load them. Clear the mark once:

```powershell
Get-ChildItem -Recurse | Unblock-File
```
