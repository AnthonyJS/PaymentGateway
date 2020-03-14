using System;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Queries
{
  public class GetPaymentByIdQuery : IRequest<Result<Payment>>
  {
    public Guid Id { get; set; }

    public GetPaymentByIdQuery(Guid id)
    {
      Id = id;
    }
  }
}
