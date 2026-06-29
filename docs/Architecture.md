# Architecture

This project follows a modular architecture for a Temporal-powered AI scribe workflow.

- API: receives requests and publishes workflow commands.
- Worker: hosts Temporal workflows and activities.
- Contracts: shared request and response models.
- Workflows: orchestration logic.
- Activities: domain-level steps.
- Infrastructure/Persistence: external integrations and storage.
- Shared: common utilities and constants.
