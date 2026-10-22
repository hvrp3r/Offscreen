# Contributing to Offscreen

Thanks for helping. This guide explains how work is organized and how a change reaches `main`.

## Finding work

- Every task is a GitHub issue, tracked in the project board with a priority (`P0`, `P1`, `P2`), a milestone and a sprint.
- Assign yourself to an issue before starting it. Issues labeled `good first issue` are a good place to start.
- Open an issue before starting work that has none.

## Branches

The repository follows GitHub Flow.

- `main` is protected and always releasable.
- Create one branch per issue from `main`, named `<type>/<issue>-<short-description>`, for example `feat/15-smb-source` or `fix/42-subtitle-offset`.
- Keep branches short-lived and rebase on `main` when it moves.

## Commits

Commits follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/):

```text
<type>(<scope>): <summary>
```

| Type | Use |
|---|---|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `test` | Tests only |
| `refactor` | Code change without behavior change |
| `perf` | Performance improvement |
| `build` | Build system, packaging, dependencies |
| `ci` | CI configuration |
| `chore` | Maintenance |

Scopes match the repository areas: `core`, `tv`, `mobile`, `extensions`, `sdk`, `themes`, `docs`, `infra`.

## Pull requests

1. Push your branch and open a pull request against `main`.
2. Link the issue with `Closes #<number>` in the description.
3. Make sure lint, tests and build pass in CI.
4. Get one approving review. Address comments with new commits.
5. The pull request is squash-merged. Its title becomes the commit on `main`, so it must follow Conventional Commits.

## Code

- Code, comments and documentation are written in English.
- Comment only what the code cannot say by itself, such as a non-obvious reason or constraint.
- Tests never call external services. External APIs are mocked so the test suite runs offline.
- Never commit secrets, media files, build outputs or dependencies.
- Every dependency must have a license compatible with AGPL-3.0.

## Documentation

The architecture book lives in `docs/src` and is translated to French in `docs/po`. The technical reference lives in `docs/technical`. See [docs/README.md](docs/README.md) for the commands.

When you change English text in the book, run `scripts/docs.sh update` and update the French translation in the same pull request.

## Versions

Releases are tagged `vMAJOR.MINOR.PATCH` on `main` following [Semantic Versioning](https://semver.org/). Each release adds an entry to [CHANGELOG.md](CHANGELOG.md).

## Code of conduct

Everyone taking part in the project follows the [code of conduct](CODE_OF_CONDUCT.md).
