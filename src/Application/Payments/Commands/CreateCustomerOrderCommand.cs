using System;
using MediatR;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Commands
{
  public class CreateCustomerOrderCommand : IRequest<OrderResponse>
  {
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
  }
}