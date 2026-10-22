#!/usr/bin/env bash
set -euo pipefail

DOCS_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../docs" && pwd)"
IMAGE="offscreen-docs"
LANGUAGES=(fr)
DOCKER_TTY="$([ -t 0 ] && echo -it || true)"

usage() {
  cat <<EOF
Usage: scripts/docs.sh <command> [language]

  serve          Build every language and serve the site on http://localhost:3000
  preview [lang] Live-reloading preview of one language (default: en)
  build          Build every language into docs/book
  extract        Extract English text into docs/po/messages.pot
  update         Refresh every translation from messages.pot
  init <lang>    Start a new translation in docs/po/<lang>.po
  stats          Show translation coverage
EOF
}

run() {
  docker image inspect "$IMAGE" >/dev/null 2>&1 || docker build -t "$IMAGE" "$DOCS_DIR"
  MSYS_NO_PATHCONV=1 docker run --rm $DOCKER_TTY -v "$DOCS_DIR:/docs" "$@"
}

extract() {
  run -e MDBOOK_OUTPUT='{"xgettext": {}}' "$IMAGE" mdbook build -d po
}

build() {
  run "$IMAGE" mdbook build -d book
  for lang in "${LANGUAGES[@]}"; do
    run -e MDBOOK_BOOK__LANGUAGE="$lang" "$IMAGE" mdbook build -d "book/$lang"
  done
}

case "${1:-}" in
  serve)
    build
    echo "Serving every language on http://localhost:3000"
    run -p 3000:3000 "$IMAGE" busybox httpd -f -p 3000 -h /docs/book
    ;;
  preview)
    lang="${2:-en}"
    run -p 3000:3000 -e MDBOOK_BOOK__LANGUAGE="$lang" "$IMAGE" \
      mdbook serve -n 0.0.0.0 -p 3000 -d "/tmp/book-$lang"
    ;;
  build)
    build
    ;;
  extract)
    extract
    ;;
  update)
    extract
    for lang in "${LANGUAGES[@]}"; do
      run "$IMAGE" msgmerge --update --backup=none "po/$lang.po" po/messages.pot
    done
    ;;
  init)
    lang="${2:?language code required}"
    extract
    run "$IMAGE" msginit --no-translator -i po/messages.pot -l "$lang" -o "po/$lang.po"
    ;;
  stats)
    for lang in "${LANGUAGES[@]}"; do
      printf '%s: ' "$lang"
      run "$IMAGE" msgfmt --statistics -o /dev/null "po/$lang.po" 2>&1
    done
    ;;
  *)
    usage
    exit 1
    ;;
esac
