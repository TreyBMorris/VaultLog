# VaultLog

VaultLog is a personal application to track watched movies, TV show episodes,
books, and video games.

## Background

I have recently taken an interest in homelabbing. I have my own homelab server
running Proxmox along with an Ubuntu VM that runs apps in docker containers
and TrueNAS.

I wanted to create an application to learn some new things, like setting up
an authentication provider (in this case Authentik) for using JWTs for authn/authz.

I also wanted to play around more with FastEndpoints and try out ValKey for caching.

The goal of this is to also be 75%+ free of AI. I might use it for reviewing or
if I really cannot figure something out, but the main goal is to use AI
tooling as little as possible.

## Purpose

Along with learning new tools, the purpose of this application is to act as
an all around review and logging platform for media. I have taken an interest
in my media server hosted with Jellyfin, and wanted to keep track of the shows,
movies, etc, that I watch. Along with this, I wanted to be able to track and
review these along with books and games. No more Letterboxd or Goodreads. Just
one central logger and reviewer.

## Goals

- Be 75%+ free of AI usage. AI can be used for code reviews and writing tests,
but will not be used for most of the code. "Old Skool" coding.
- Learn new tools
- Learn to create a deployable app in Docker image that can be hosted in GHCR or
something similar. (Or something that can be easily deployed on my homelab)
- Potentially be open for OSS contributions. Not sure about this one,
but an interesting idea no doubt.

## Tech Stack

- .NET
  - FastEndpoints - Endpoint routing to replace controllers
  - EF Core - ORM
  - PostgreSQL - Database
  - FluentValidation

- Angular
  - Chart.js

- Tools
  - Valkey - Caching
  - Authentik - SSO and JWT for the API
  - Docker

- External APIs
  - TMDB - Movies and TV shows
  - IGDB - Games
  - Open Library - Books
