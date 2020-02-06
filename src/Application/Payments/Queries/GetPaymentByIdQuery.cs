using System;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Queries
{
  public class GetPaymentByIdQuery : IRequest<Result<PaymentByIdResponse>>
  {
    public Guid Id { get; set; }

    public GetPaymentByIdQuery(Guid id)
    {
      Id = id;
    }
  }
}