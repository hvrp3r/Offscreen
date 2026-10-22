#!/usr/bin/env bash
set -euo pipefail

CLIENTS_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../clients" && pwd)"
cd "$CLIENTS_DIR"

usage() {
  cat <<USAGE
Usage: scripts/clients.sh <command>

  get       Resolve dependencies for every app and package
  analyze   Run the analyzer on the whole workspace
  format    Format the sources, or check them with --check
  test      Run the tests of every app and package
USAGE
}

members() {
  sed -n '/^workspace:/,/^[^ ]/s/^  - //p' pubspec.yaml
}

case "${1:-}" in
  get)
    flutter pub get
    ;;
  analyze)
    flutter pub get
    flutter analyze
    ;;
  format)
    if [ "${2:-}" = "--check" ]; then
      dart format --output=none --set-exit-if-changed $(members)
    else
      dart format $(members)
    fi
    ;;
  test)
    flutter pub get
    for member in $(members); do
      if [ -d "$member/test" ]; then
        echo "== $member"
        (cd "$member" && flutter test)
      fi
    done
    ;;
  *)
    usage
    exit 1
    ;;
esac
