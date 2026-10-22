# Repository layout

> Status: draft.

The repository is a monorepo. Each top-level folder builds and tests on its own, and one root command runs everything.

```text
.
├── core/                         .NET solution
│   ├── src/
│   │   ├── Offscreen.Domain/
│   │   ├── Offscreen.Application/
│   │   ├── Offscreen.Infrastructure/
│   │   ├── Offscreen.Sources.Local/
│   │   ├── Offscreen.Sources.Smb/
│   │   ├── Offscreen.Extensions/
│   │   ├── Offscreen.Api/
│   │   └── Offscreen.Host/
│   └── tests/
│       ├── Offscreen.Domain.Tests/
│       ├── Offscreen.Application.Tests/
│       ├── Offscreen.Api.Tests/
│       └── fixtures/             filename recognition cases, sample manifests
├── clients/                      Flutter workspace
│   ├── packages/
│   │   ├── offscreen_api/
│   │   ├── offscreen_ui/
│   │   ├── offscreen_player/
│   │   └── offscreen_l10n/
│   └── apps/
│       ├── tv/
│       └── mobile/
├── sdk/
│   ├── schemas/                  JSON Schemas: extension manifest, theme
│   └── typescript/               extension SDK for TypeScript authors
├── extensions/                   extensions maintained in this repository
│   ├── tmdb/
│   ├── musicbrainz/
│   ├── lrclib/
│   ├── internet-archive/
│   ├── tvmaze/
│   └── sleep-timer/
├── themes/
│   ├── dark/
│   └── light/
├── samples/                      sample library manifest and licenses, no media files
├── scripts/                      setup, sample library download, packaging helpers
├── docs/                         mdBook sources
└── .github/                      workflows, issue and pull request templates
```

## Rules

- Media files, build outputs and dependencies are never committed.
- Each extension folder contains its manifest, sources, tests and a README.
- Shared contracts live in `sdk/schemas` and are validated in CI against every extension and theme in the repository.
