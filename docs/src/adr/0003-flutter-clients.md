# ADR-0003 Flutter clients

**Context.** The TV client must run full screen on Linux and Windows and play through a real playback library. The remote must be a native Android app.

**Options.**
- Qt/QML: excellent libmpv integration, no shared code with mobile.
- Tauri or Electron: web UI, libmpv only in a separate window.
- Avalonia: C# everywhere, weaker mobile support.
- Flutter: libmpv through media_kit, one codebase for desktop and Android.

**Decision.** Flutter for both clients.

**Consequences.** One design system and one API client for every screen. Screen reader support on Linux must be checked early. The fallback is built-in speech output of the focused element.
