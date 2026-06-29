# Temporal AI Scribe POC

This repository now contains the initial foundation for a Temporal-powered AI scribe solution with:

- a .NET solution and multi-project backend layout
- an ASP.NET Core API and background worker
- contracts, workflows, activities, infrastructure, persistence, and shared libraries
- a React + Vite UI shell
- Docker Compose setup for PostgreSQL and Temporal
- starter documentation for architecture and demo flow

## Structure

- [src/TemporalAiScribe.Api](src/TemporalAiScribe.Api)
- [src/TemporalAiScribe.Worker](src/TemporalAiScribe.Worker)
- [src/TemporalAiScribe.Contracts](src/TemporalAiScribe.Contracts)
- [src/TemporalAiScribe.Workflows](src/TemporalAiScribe.Workflows)
- [src/TemporalAiScribe.Activities](src/TemporalAiScribe.Activities)
- [src/TemporalAiScribe.Infrastructure](src/TemporalAiScribe.Infrastructure)
- [src/TemporalAiScribe.Persistence](src/TemporalAiScribe.Persistence)
- [src/TemporalAiScribe.Shared](src/TemporalAiScribe.Shared)
- [ui/temporal-ai-ui](ui/temporal-ai-ui)
- [docker/docker-compose.yml](docker/docker-compose.yml)

## Run locally

### Backend

```bash
dotnet build TemporalAiScribe.slnx
dotnet run --project src/TemporalAiScribe.Api/TemporalAiScribe.Api.csproj
```

### UI

```bash
cd ui/temporal-ai-ui
npm install
npm run dev
```

### Docker

```bash
docker compose -f docker/docker-compose.yml up
```
