# Architecture

## Components

```text
┌──────────────────────────── Core (.NET) ────────────────────────────┐
│  Library ── Recognition ── Metadata cache        Profiles           │
│  Sources (local, SMB, NFS)                       Parental control   │
│  Playback orchestration ── Queue ── Progress                        │
│  Extension host (WebAssembly sandbox)            Paired devices     │
│  Control API (HTTP + WebSocket) ── Media streaming ── mDNS          │
└──────────────────────────────────┬──────────────────────────────────┘
                                   │
                 ┌─────────────────┴─────────────────┐
        ┌────────┴────────┐                 ┌────────┴────────┐
        │ TV client       │                 │ Mobile remote   │
        │ Flutter + mpv   │                 │ Flutter Android │
        └─────────────────┘                 └─────────────────┘
```

| Component | Role |
|---|---|
| Core | Headless service that owns the library, profiles, progress, extensions and paired devices. |
| TV client | Ten-foot interface and playback through libmpv. Ships with the core. |
| Mobile remote | Pairs with the core, controls playback and browses the library. |
| Extensions | Packages loaded at runtime in a sandbox, limited to the permissions the user granted. |
| Themes | Data packages switched without restart. |

## Data flows

**Scan and enrichment.** A source is listed, compared with the last snapshot, and only added, changed or removed files are processed. New files are recognized from their names and tags, then enriched by metadata providers. Results and images are cached so the library works offline.

**Playback.** A client asks the core to play an item. The core checks parental control, finds the resume position and tells the TV client to play a stream URL served by the core. The TV client reports its state, and the core saves progress and broadcasts it to paired devices.

**Remote control.** The phone finds the core through mDNS, pairs with a code shown on the TV, then sends commands over HTTP and receives live state over WebSocket.

**Extensions.** The core calls an extension inside its sandbox with a time and memory limit. Each call the extension makes back to the core is checked against its permissions. A failing extension is stopped without affecting the core.

## Stack

| Part | Choice | Why |
|---|---|---|
| Core | C# / .NET | Mature SMB and WebAssembly libraries, single binary for Linux and Windows. See [ADR-0002](adr/0002-dotnet-core.md). |
| Clients | Flutter, media_kit (libmpv) | One codebase for the TV client and the Android remote, real playback library. See [ADR-0003](adr/0003-flutter-clients.md). |
| Extensions | WebAssembly (Extism) | Strong isolation that maps directly to permissions. See [ADR-0004](adr/0004-wasm-extensions.md). |
| Storage | SQLite | Embedded, no server to install. |
| Network shares | SMB, then NFS and WebDAV | SMB covers most homes. NFS is suggested first when the host offers it. See [ADR-0005](adr/0005-network-protocols.md). |
