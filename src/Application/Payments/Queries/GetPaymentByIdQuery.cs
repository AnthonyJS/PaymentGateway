using System;
using MediatR;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Queries
{
  public class GetPaymentByIdQuery : IRequest<PaymentResponse>
  {
    public Guid Id { get; set; }

    public GetPaymentByIdQuery(Guid id)
    {
      Id = id;
    }
  }
}