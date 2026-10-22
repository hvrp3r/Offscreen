# ADR-0001 Headless core

**Context.** The media center runs on the TV computer and is controlled from a phone.

**Options.**
- A desktop application that also exposes a control API.
- A headless core owning all state, with clients that display and play.

**Decision.** A headless core. The TV client ships with it, so a single install is enough.

**Consequences.** Every feature goes through the API, which keeps the TV client and the remote consistent. Media is streamed by the core, so share credentials never leave it.
