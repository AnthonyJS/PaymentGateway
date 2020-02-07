## TODO

- Unit testing
- Readme
- Merge into master

- Validation
- Handle ABC as currency
- AuditableEntity base class
- Custom exceptions
- Docker

## Assumptions

## Technologies and libraries

- MediatR
- Automapper
- LiteDB
- [CSharpFunctionalExtensions](https://www.nuget.org/packages/CSharpFunctionalExtensions/)

## Architecture and design decisions

- CQRS
- Onion architecture
  - API
  - Application
  - Domain (omitted)
  - Infrastructure

## Further areas for development

- Put call to Acquiring Bank and saving to DB in a transaction to ensure it is atomic, and can rollback if failure occurs part way through.

- Store LiteDB in Docker volume to make it persist
