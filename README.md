# OblivionMP Mod Template

![version](https://img.shields.io/badge/version-0.2.0-green)

For other versions, check the list of [tags](https://github.com/readycodeio/oblivionmp-mod-template/tags).

A template for developing an OblivionMP mod with the OblivionMP SDK.

Refer to the [OblivionMP SDK documentation](https://docs.ready.mp) for details on how to use the SDK.

## Requirements

* [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) or later
* If you are using Visual Studio, you need version 2026 or later

## Repository structure

An OblivionMP mod is usually made of two halves that share a common core:

- `ExampleMod.Common/`: shared code that both halves need. Networked component definitions
  (`WalletComponent.cs`) and server-RPC contracts (`RpcContracts.cs`) live here. Its DLL ships
  alongside both halves.
- `ExampleMod.Client/`: the client-side half, loaded by the game. Entry point (`Mod.cs`), the client
  side of server RPC (`ExampleServerRpc.cs`) and a client-relayed RPC (`ExampleClientRpc.cs`).
- `ExampleMod.Server/`: the server-side half, loaded by the relay server. Entry point (`Mod.cs`), the
  server side of server RPC (`ExampleServerRpc.cs`), and a gameplay system (`PassiveIncomeSystem.cs`).
- `Content/manifest.json`: mod metadata (id, name, version, dependencies).
- `Dependencies/`: the OblivionMP SDK assemblies the projects compile against, dumped from the SDK
  build. `Client/` holds what the game provides, `Server/` what the relay server provides. The
  projects reference them with `Private=false`, so they are never copied into your mod.

The example is a small "wallet" feature: a networked `WalletComponent` on every player, an RPC to add
gold and to read the balance, and a server system that grants passive income. Use it as a starting
point and replace it with your own logic.

## Getting started

1. Clone this repository to your local machine.
2. Open `ExampleMod.sln` in your preferred C# IDE (e.g. JetBrains Rider, Visual Studio).
3. Build the solution to ensure all dependencies resolve.
4. Rename the projects and edit the code to build your own mod. Keep shared components and RPC
   contracts in `ExampleMod.Common` so both halves agree on them.

> **Note**: A networked component's shape and its archetype membership must match between the two
> halves. Ship them together as one package, which is what the packaging script produces.

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
