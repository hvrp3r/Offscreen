# Themes

> Status: draft.

## Themes

A theme is a data-only package (`types: ["theme"]`) containing:

| Part | Content |
|---|---|
| Tokens | Colors, typography scale, spacing, corner radius, elevation, focus ring style, motion durations |
| Home layout | Ordered list of rows: continue watching, recently added, movies, series, music, podcasts, extension widgets |
| Assets | Fonts and images, each with its license |

Rules:

- Themes are validated at install: every text and background token pair must meet a 4.5:1 contrast ratio, and the focus ring must reach 3:1 against its surroundings.
- Switching the theme applies immediately, without a restart.
- Each profile stores its own theme. The default is the dark theme.
- Reduced motion in the profile overrides the theme's motion durations.

Shipped themes: `dark` (purple accent, default) and `light`.
