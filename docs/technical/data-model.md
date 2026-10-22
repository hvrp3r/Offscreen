# Data model

> Status: draft. The field dictionary will be completed with the first migrations.

## Entities

```text
Source 1───* MediaFile *───1 MediaItem
                                 │
          ┌──────────┬───────────┼────────────┬─────────────┬──────────────┐
        Movie     Series      Episode       Album         Track        PodcastEpisode
                    │ 1         * │           │ *           * │              * │
                    └──* Season ─┘           Artist ─────────┘           Podcast

MediaItem 1───* Artwork          MediaItem *───* Genre        MediaItem *───* Person (Credit)
MediaItem 1───* ProviderLink     MediaItem 1───* MediaStream (audio, subtitle, video tracks)

Profile 1───* PlaybackProgress *───1 MediaItem
Profile 1───* Favorite         *───1 MediaItem
Profile 1───* Playlist 1───* PlaylistEntry *───1 MediaItem
Profile 1───1 ProfilePreferences

Device (paired)          Extension 1───* PermissionGrant      Extension 1───* ExtensionSetting
Theme                    LocalePack                           SourceCredential (reference into ISecretStore)
```

`MediaItem` is the common table for everything a user can browse. Type-specific data lives in one table per type sharing the same id. A `MediaFile` is a physical file on a source. Keeping files separate from items lets a moved or replaced file keep its history, and lets one item have several versions.

## Key fields

| Entity | Fields |
|---|---|
| Source | id, kind (`local`, `smb`, …), display name, root path or URI, credential reference, availability (`online`, `degraded`, `unreachable`, `removed`), last scan date |
| MediaFile | id, source id, relative path, size, modification time, quick hash, container, duration, last seen date, missing flag |
| MediaItem | id, kind, title, sort title, original title, year, overview, runtime, rating value, content rating (normalized age), added date, locked fields, recognition confidence |
| Episode | series id, season number, episode number, absolute number, air date |
| Track | album id, artist ids, disc, number, duration, lyrics availability |
| PodcastEpisode | podcast id, guid, publish date, enclosure URL, downloaded file id |
| ProviderLink | media item id, provider extension id, external id |
| Artwork | media item id, kind (poster, backdrop, thumb, cover), cached path, source URL, width, height |
| Profile | id, name, avatar, kind (`standard`, `kids`), PIN hash, maximum age rating, admin flag |
| ProfilePreferences | language, theme id, subtitle style, preferred audio and subtitle languages, reduced motion, text scale, built-in speech |
| PlaybackProgress | profile id, media item id, position, duration, watched flag, play count, last played date |
| Device | id, name, platform, token hash, certificate fingerprint, paired date, last seen date, revoked flag |
| Extension | id, version, types, enabled flag, install date, crash count, state (`running`, `stopped`, `crashed`, `disabled`) |
| PermissionGrant | extension id, permission, scope (for example allowed hosts), granted date |

## Conventions

- Identifiers are UUIDv7, so they sort by creation time.
- Dates are stored in UTC.
- The user's manual corrections are stored as locked fields, so enrichment never overwrites them.
- Content ratings from different countries are normalized to a minimum age, which parental control compares against.
