# Documentation

| Folder | Content |
|---|---|
| `src/` | Architecture book, built with mdBook and translated to French |
| `technical/` | Technical reference: data model, library, playback, extensions, themes, control API, repository layout |
| `po/` | Translations of the book |

## Publishing

The `Documentation` workflow builds the book in every language on each pull request that touches it, and reports translation coverage in the run summary. Every push to `main` publishes it to https://hvrp3r.github.io/Offscreen/.

## Commands

Run from the repository root. Docker is required.

| Command | Effect |
|---|---|
| `scripts/docs.sh serve` | Build every language and serve the book on http://localhost:3000 |
| `scripts/docs.sh preview [lang]` | Live preview of one language |
| `scripts/docs.sh update` | Refresh translations after an English change |
| `scripts/docs.sh stats` | Show translation coverage |
