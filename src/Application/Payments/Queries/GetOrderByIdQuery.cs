using System;
using MediatR;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Queries
{
  public class GetOrderByIdQuery : IRequest<OrderResponse>
  {
    public Guid Id { get; set; }

    public GetOrderByIdQuery(Guid id)
    {
      Id = id;
    }
  }
}