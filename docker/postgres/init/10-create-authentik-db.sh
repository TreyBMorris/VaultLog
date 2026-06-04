#!/bin/bash
# Creates a separate database + user for Authentik in the same Postgres server.
# Runs once, on first container start (empty data dir).
set -euo pipefail

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER authentik WITH PASSWORD '${AUTHENTIK_DB_PASSWORD}';
    CREATE DATABASE authentik OWNER authentik;
EOSQL
