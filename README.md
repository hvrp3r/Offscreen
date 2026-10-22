# Offscreen

Offscreen is a free and open source media center. It organizes your movies, series, music and podcasts from local folders and network shares, enriches them with posters, summaries and cast, plays them on a TV-friendly interface, and is controlled from your phone.

> Offscreen is in early development. The sections below describe what is available today.

## Features

- Living-room interface navigable with arrows, Enter and Back
- Local folders and network shares
- Automatic recognition and metadata enrichment, usable offline
- Profiles with history, favorites, progress and parental control
- Extensions running in a sandbox, with permissions approved by the user
- Interchangeable themes
- Android remote with automatic discovery and secure pairing

## Repository

| Folder | Content |
|---|---|
| [`docs/`](docs/) | Architecture book, technical reference and documentation tooling |
| [`scripts/`](scripts/) | Development scripts |

The full layout planned for the project is described in [docs/technical/repository-layout.md](docs/technical/repository-layout.md).

## Prerequisites

| Tool | Used for |
|---|---|
| [Git](https://git-scm.com/) | Source control |
| [Docker](https://www.docker.com/) | Building and previewing the documentation |

## Documentation

The documentation is published at https://hvrp3r.github.io/Offscreen/, in English and French.

Build every language and serve the documentation on http://localhost:3000:

```bash
scripts/docs.sh serve
```

- [Architecture](docs/src/architecture.md)
- [Architecture decisions](docs/src/adr/README.md)
- [Technical reference](docs/technical/)

## Contributing

Read the [contribution guide](CONTRIBUTING.md) and the [code of conduct](CODE_OF_CONDUCT.md). The roadmap and open tasks are in the [project board](https://github.com/users/hvrp3r/projects/5).

## License

Offscreen is licensed under the [GNU Affero General Public License v3.0 or later](LICENSE).
