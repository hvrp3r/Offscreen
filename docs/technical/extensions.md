# Extensions

> Status: draft.

An extension adds a capability to Offscreen without changing its code. Anyone can write one, so the core treats every extension as untrusted.

## Package

An extension is a `.offx` file, a zip archive:

```text
manifest.json
plugin.wasm          absent for data-only packages
locales/en.json
locales/fr.json
assets/              icons, images
LICENSE
```

### Manifest

```json
{
  "id": "org.offscreen.sleep-timer",
  "name": "Sleep timer",
  "version": "1.0.0",
  "apiVersion": "^1.0",
  "author": "Offscreen contributors",
  "description": "Stops playback after a chosen delay.",
  "license": "AGPL-3.0-or-later",
  "types": ["service", "widget"],
  "permissions": ["playback.read", "playback.control", "ui.home", "storage"],
  "events": ["playback.started", "playback.stopped"],
  "settings": "settings.schema.json"
}
```

The manifest is validated against `sdk/schemas/manifest.schema.json` at install time and in CI.

## Types

| Type | Contract | Examples |
|---|---|---|
| `source` | Lists browsable collections and items, resolves a playable URL. | Internet Archive public domain films |
| `metadata` | Searches and returns details and artwork for a media kind. | TMDB, MusicBrainz, TVmaze, LRCLIB |
| `widget` | Returns a declarative UI tree for the home screen. | Sleep timer |
| `service` | Reacts to events and runs scheduled tasks. | Sleep timer |
| `theme` | Data only. See [Themes](themes.md). | Dark, light |

An extension can declare several types.

## Permissions

| Permission | Grants |
|---|---|
| `network` | Outgoing HTTP, limited to the hosts listed in the manifest |
| `library.read` | Read library items and metadata |
| `library.write` | Add or edit items provided by this extension |
| `playback.read` | Receive playback events and state |
| `playback.control` | Send playback commands |
| `profile.read` | Read the active profile's non-sensitive preferences |
| `storage` | Private key-value storage with a quota |
| `ui.home` | Contribute a widget to the home screen |
| `notifications` | Show notifications |

The user sees the requested permissions at install and on every update that asks for more. Anything not granted is unavailable: the host function for an ungranted permission is not linked into the sandbox, so the call fails with `permission_denied`.

## Runtime

- Each extension runs in its own Extism plugin instance (WebAssembly), hosted by the core.
- Every call has a timeout and the instance has a memory ceiling, both set by the host.
- A trap, a timeout or repeated excess resource use stops the instance, emits `extension.crashed` and notifies the user. The host restarts it with exponential backoff, and disables it after repeated failures.
- Install, update, enable, disable and uninstall happen at runtime. The core emits `extensions.changed` and clients refresh without a restart.
- The host checks `apiVersion` and refuses packages built for an incompatible API.

## UI contributions

Extensions never ship UI code. Widgets and settings are declarative:

- **Widgets** return a tree of a small set of components (row, column, card, text, image, button, progress, list). The Flutter clients render it with the active theme, focus navigation and screen reader semantics.
- **Settings** are a JSON Schema subset. Clients generate the form.

Because the host renders everything, every extension inherits the accessibility and theming of the product.

## Authoring and debugging

Authors write TypeScript against `@offscreen/sdk` and never deal with WebAssembly or Extism directly. The SDK runs on QuickJS inside the sandbox, so Node and browser APIs are unavailable: network, storage and every other capability go through the SDK.

| Command | Effect |
|---|---|
| `npx offscreen build` | Compiles the extension to WebAssembly with source maps and packages the `.offx` file. |
| `npx offscreen dev` | Runs the extension in Node, connected to a core started in development mode. Host calls reach the real core, which still enforces the manifest permissions. Breakpoints work in the original TypeScript and the extension reloads on save. |
| `npx offscreen test` | Runs the author's tests against a mocked core, first in Node, then against the WebAssembly build to catch differences between Node and QuickJS. |

In production, traps and uncaught errors are logged by the core with the extension id and a stack trace mapped back to the TypeScript sources. `log.debug` and `log.error` from the SDK write to the same logs.

## Distribution

Users install an extension from a file or from a catalog. A catalog is an index file listing packages, versions, checksums and permissions. The product ships with a local catalog of the extensions maintained in this repository.

## Preinstalled extensions

| Extension | Type | Notes |
|---|---|---|
| TMDB | metadata | Movies and series. The user enters their own API key in the first-launch wizard. |
| MusicBrainz + Cover Art Archive | metadata | Music. No key, identified user agent, one request per second. |
| LRCLIB | metadata | Plain and synchronized lyrics. |

## Delivered extensions

| Extension | Type |
|---|---|
| Internet Archive and Blender open movies | source |
| TVmaze | metadata |
| Sleep timer | service, widget |
