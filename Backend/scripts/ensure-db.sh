#!/usr/bin/env bash
set -euo pipefail

# Path vars
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
BACKEND_DIR="$ROOT_DIR"

echo "[ensure-db] Starting Docker Compose..."
docker-compose up -d

cd "$BACKEND_DIR"

MAX_RETRIES=15
SLEEP_SECONDS=4
i=0
echo "[ensure-db] Running 'dotnet ef database update' with up to $MAX_RETRIES retries..."
until dotnet ef database update; do
  i=$((i+1))
  if [ $i -ge $MAX_RETRIES ]; then
    echo "[ensure-db] Failed to apply migrations after $MAX_RETRIES attempts." >&2
    exit 1
  fi
  echo "[ensure-db] Migration attempt $i failed — waiting $SLEEP_SECONDS seconds before retry..."
  sleep $SLEEP_SECONDS
done

echo "[ensure-db] Migrations applied successfully."
