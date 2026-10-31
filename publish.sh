#!/usr/bin/env bash
set -eu
cd "$(dirname "$0")"
dotnet publish WindowsTimerApp.csproj -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -o publish_single_file
