# Control API

> Status: draft. The full reference is generated from the OpenAPI document.

## Transport

| Channel | Use |
|---|---|
| `https://<host>:<port>/api/v1/...` | Commands and queries (REST, JSON) |
| `wss://<host>:<port>/api/v1/events` | Real-time events and renderer commands |
| `GET /api/v1/stream/{fileId}` | Media streaming with range requests |

The core uses a self-signed certificate generated at first start. Clients pin its fingerprint during pairing.

WebSocket messages share one envelope:

```json
{ "type": "playback.state", "id": "0192f6e4-…", "payload": { } }
```

## Discovery

The core announces `_offscreen._tcp` over mDNS, with TXT records `id`, `name`, `version` and `fp` (certificate fingerprint). Clients can also connect to a manually entered address.

## Pairing

1. The new device asks to pair. The TV shows a six-digit code and a QR code containing the address, the code and the certificate fingerprint.
2. The device sends the code. Codes expire after two minutes and attempts are rate-limited.
3. The core returns a device token: 256 random bits, stored hashed on the core.
4. Every request carries the token. Devices can be renamed or revoked from the TV.

In all-in-one mode the bundled client receives a local token through a file readable only by the current user, so no pairing is needed.

## Resources

| Area | Examples |
|---|---|
| Library | browse, search, filter, item details, fix a recognition |
| Profiles | list, select with PIN, preferences |
| Renderers | list renderers, play on a renderer, current state |
| Playback | play, pause, seek, stop, next, previous, tracks, subtitle style, volume |
| Queue | read, add, move, remove, clear, shuffle, repeat |
| Navigation | `up`, `down`, `left`, `right`, `select`, `back`, `home`, `menu`, forwarded to the renderer |
| Send to TV | play a compatible URL or upload a music file from the phone |
| Sources | add, test, discover, rescan |
| Extensions | catalog, install, update, configure, enable, disable, uninstall |
| Devices | list, rename, revoke |

## Events

`playback.state`, `playback.progress`, `queue.changed`, `library.changed`, `source.availability`, `extensions.changed`, `extension.crashed`, `profile.changed`, `notification`.
