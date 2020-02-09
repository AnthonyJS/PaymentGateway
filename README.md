# Payment Gateway demo

## Steps to get Payment Gateway running locally

- Clone this repository

### To run the API

- From the root directory, type the following on the CLI:

  `dotnet run --project src/API/API.csproj`

### To run the tests

- From the root directory, type the following on the CLI:

  `dotnet test`

## Using the API

### Making a payment

Send an `HttpPost` to

```text
https://localhost:5001/paymentgateway/
```

#### Body

```JSON
{
    "firstName": "Tim",
    "surname": "Tomson",
    "cardNumber": "1234-5678-8765-4321",
    "expiryMonth": 10,
    "expiryYear": 20,
    "currency": "GBP",
    "amount": 4404.44,
    "cvv": 321
}
```

This will return as `id`

```JSON
{
    "id": "9e51d18a-e022-4aed-8d62-01a2cc0bea7d"
}
```

## TODO

- Unit testing
- Readme
- Merge into master

- Validation
- Logging
- Handle ABC as currency
- AuditableEntity base class
- Custom exceptions
- Docker
- Add `ProducesResponseType` to controllers

## Assumptions

## Technologies and libraries

- MediatR
- Automapper
- LiteDB
- [CSharpFunctionalExtensions](https://www.nuget.org/packages/CSharpFunctionalExtensions/)
- XUnit

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

- Swagger / Swagger UI

```

```
