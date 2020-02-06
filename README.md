## TODO

- Result<T>
- HTTP status codes? Some method of returning success / failure to use as code
- AuditableEntity base class
- Readme
- Merge into master
- Unit testing
- Validation
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

Put call to Acquiring Bank and saving to DB in a transaction to ensure it is atomic, and can rollback if failure occurs part way through.
