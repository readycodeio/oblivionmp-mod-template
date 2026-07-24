# OblivionMP Mod Template

![version](https://img.shields.io/badge/version-0.1.0-green)

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
  alongside both the client and the server mod.
- `ExampleMod.Client/`: the client-side mod, loaded by the game. Entry point (`Mod.cs`), the client
  side of server RPC (`ExampleServerRpc.cs`), a client-relayed RPC (`ExampleClientRpc.cs`), and the
  archetype registration (`ExampleRegistration.cs`).
- `ExampleMod.Server/`: the server-side mod, loaded by the relay server. Entry point (`Mod.cs`), the
  server side of server RPC (`ExampleServerRpc.cs`), and a gameplay system (`PassiveIncomeSystem.cs`).
- `Content/manifest.json`: metadata for the client mod (name, version, dependencies).
- `Dependencies/`: the OblivionMP SDK assemblies the projects reference. The same files ship in the
  server binary package. `Client/` is referenced by the client and shared projects; `Server/` by the
  server project.

The example is a small "wallet" feature: a networked `WalletComponent` on every player, an RPC to add
gold and to read the balance, and a server system that grants passive income. Use it as a starting
point and replace it with your own logic.

## Getting started

1. Clone this repository to your local machine.
2. Open `ExampleMod.sln` in your preferred C# IDE (e.g. JetBrains Rider, Visual Studio).
3. Build the solution to ensure all dependencies resolve.
4. Rename the projects and edit the code to build your own mod. Keep shared components and RPC
   contracts in `ExampleMod.Common` so both halves agree on them.

> **Note**: A networked component's shape and its archetype membership must match between the client
> mod and the server mod. Ship both halves together as versions of the same package.

## Packaging the mod

1. Edit `Content/manifest.json` with your mod's name, version, and description.
2. Edit `ModFiles.ps1` if your mod ships extra files or you renamed the projects.
3. Run the packaging script with the `Release` argument:

   ```powershell
   .\MakeModFolder.ps1 Release
   ```

4. The `Output` directory will contain two folders:
   - `ExampleMod.Client/`: copy this into your server's `mods/` directory (as a folder or a `.zip`).
   - `ExampleMod.Server/`: copy this into your server's `server_mods/` directory.
5. Restart the server. Connecting clients download the client mod automatically.

## Debugging

Run the packaging script with the `Debug` argument to include `.pdb` symbol files:

```powershell
.\MakeModFolder.ps1 Debug
```
