#!/usr/bin/env bash
set -eu
cd "$(dirname "$0")"
bash publish.sh
ISCC installer/WindowsTimerApp.iss
