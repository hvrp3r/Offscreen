# Media library

> Status: draft.

## Sources

A source provider implements `ISourceProvider`:

| Operation | Purpose |
|---|---|
| `Probe` | Check reachability and latency, used to compute availability. |
| `Enumerate` | Stream entries (path, size, modification time) under a root. |
| `Open` | Return a seekable stream for a file, used by the streaming endpoint. |
| `Watch` (optional) | Push change notifications when the protocol supports them. |
| `Discover` (optional) | Find shares on the local network. |

Providers are built in this order: local folders and SMB, then NFS, then WebDAV. DLNA/UPnP may follow. SMB and NFS need raw network connections, so they are core modules. HTTP-based protocols can also be provided by extensions.

### Protocol selection

1. The user enters a host or picks one discovered on the network.
2. The core probes the protocols the host offers.
3. NFS is suggested first when available, SMB otherwise. The user can pick any offered protocol.
4. The source stays bound to that protocol, because paths and authentication differ between protocols.

NFS uses a read-only NFSv3 client. Linux NFS servers accept only privileged client ports by default, so the Linux packages grant the core the `CAP_NET_BIND_SERVICE` capability. When an NFS connection is refused, the user is offered SMB with an explanation.

### Availability

Each source keeps an availability state, refreshed by probes and by I/O errors.

| State | Behavior |
|---|---|
| `online` | Normal. |
| `degraded` | Slow responses. Scans are throttled and the UI shows a notice. |
| `unreachable` | Items stay browsable from the cache, playback is disabled with a clear message, probes retry with backoff. |
| `removed` | The user deleted the source. Items are hidden but their history is kept for a grace period. |

If a file disappears during playback, the streaming endpoint reports the failure. The renderer shows an error with a retry option, and the core saves the last known position.

## Scanning

The scanner compares the listing with the stored snapshot, using the key `(source, relative path)` and the change signal `(size, modification time)`.

| Case | Detection | Result |
|---|---|---|
| Added | path not in snapshot | recognize, then enrich |
| Modified | size or modification time changed | re-probe streams, keep the item |
| Moved | a missing file and a new file share size and quick hash | update the path, keep history |
| Removed | path missing from the listing | mark the file missing, then delete it after a grace period |

The quick hash covers the first and last 64 KiB plus the size. It is cheap over the network and good enough to pair moves.

Scans run in a background queue with one worker per source, so a slow share never blocks the others.

## Recognition

Recognition is an ordered pipeline of rules. Each rule receives the path and the current candidate, and can add facts with a confidence value.

1. Normalization: strip release tags, replace separators, detect the language.
2. Episode rules: `S01E02`, `1x02`, `Season 1/Episode 2`, absolute numbering, date-based episodes.
3. Movie rules: title followed by a year, folder name as a fallback.
4. Audio: tags (ID3, Vorbis comments, MP4 atoms) take precedence over the path.
5. Local metadata: `.nfo` files and embedded tags.

Adding a rule means adding a class and a set of cases in `core/tests/fixtures/recognition`. The fixture file is the specification: each case is a path and an expected result, including deliberately tricky names.

The candidates with low confidence are flagged in the UI for manual correction. The user picks another search result or edits the fields, which then become locked.

## Enrichment

Metadata providers are extensions implementing the `metadata` contract. The user orders them per media kind. TMDB is first for movies and series, MusicBrainz for music, LRCLIB for lyrics.

- Each provider has a rate limit taken from its manifest and respected by the host.
- Responses are cached with a time-to-live, so the library is fully usable offline.
- Artwork is downloaded once, resized to a few standard sizes and stored in the cache folder.
- When every provider fails, the item keeps the data from recognition and is retried later.
- Attribution required by each provider is displayed on the item details screen and in the about screen.
