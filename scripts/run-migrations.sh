#!/usr/bin/env bash
# Auto-run database migrations for the dev container.
# Waits for PostgreSQL, runs all DbMigrator projects.
# Errors are logged but never exit(1) — dev container must always start.

set -o pipefail

LOG_DIR="$(pwd)/.migrations"
mkdir -p "$LOG_DIR"

log() {
  local msg
  msg="$(date '+%Y-%m-%d %H:%M:%S') - $*"
  echo "$msg" | tee -a "$LOG_DIR/run-migrations.log"
}

failures=0

run_migrator() {
  local name="$1"
  local path="$2"
  log "→ Running $name migrations..."
  if dotnet run --project "$path" 2>&1 | tee -a "$LOG_DIR/$name.log"; then
    log "✓ $name migrations completed."
  else
    log "✗ $name migrations FAILED (see $name.log)."
    failures=$((failures + 1))
  fi
}

log "=== Database Migrations Started ==="

# Wait for PostgreSQL (up to 60s)
log "Waiting for PostgreSQL at postgres:5432..."
for i in $(seq 1 60); do
  if pg_isready -h postgres -p 5432 -U postgres &>/dev/null; then
    log "PostgreSQL is ready."
    break
  fi
  sleep 1
done

if ! pg_isready -h postgres -p 5432 -U postgres &>/dev/null; then
  log "✗ PostgreSQL did not become ready within 60 seconds — proceeding anyway."
fi

# Run each migrator sequentially (self-contained executables, ~1-5s each)
run_migrator "Clinics"           "./Services/Clinics/Clinics.DbMigrator"
run_migrator "Doctors"           "./Services/Doctors/Doctors.DbMigrator"
run_migrator "Timetable"         "./Services/Timetable/Timetable.DbMigrator"

log "=== Database Migrations Finished ($failures failure(s)) ==="

# Always return 0 so this never blocks the dev container
exit 0
