# Playback

> Status: draft.

## Where playback happens

Playback runs inside a renderer client, never in the core and never in a browser's built-in media element. The core streams bytes and orchestrates.

## Streaming

The core serves every file through `GET /api/v1/stream/{fileId}` with HTTP range requests. The URL carries a short-lived signed token, so players that cannot add headers can still use it.

Streaming through the core in every mode means:

- clients never receive share credentials;
- local, SMB and future sources behave identically for players;
- on-the-fly transcoding can later be added behind the same endpoint.

External subtitles are served the same way.

## Player contract

`offscreen_player` defines the contract every player implements:

| Group | Operations and events |
|---|---|
| Lifecycle | `open(url, options)`, `close()`, `state` stream |
| Transport | play, pause, seek, rate, stop |
| Tracks | list audio, subtitle and video tracks; select; add external subtitle |
| Subtitles | size, color, background, position, delay |
| Audio | volume, mute, audio delay |
| Reporting | position, duration, buffering, errors, end of media |

The default implementation uses media_kit, which embeds libmpv and supports hardware decoding.

## Behaviors

| Behavior | Rule |
|---|---|
| Resume | Progress is saved every 10 seconds, on pause and on stop. An item counts as watched beyond 90 % or when the credits start. |
| Next episode | At the end of an episode, the next one starts after a countdown that can be canceled. |
| Music | Queue, shuffle and repeat are handled by the core so every device sees the same queue. |
| Podcasts | Episodes stream from their feed or play from a downloaded copy stored as a local source. |
| Accessibility | Subtitle style and audio description tracks follow the profile preferences. |
