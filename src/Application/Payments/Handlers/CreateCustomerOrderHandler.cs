using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class CreateCustomerOrderHandler : IRequestHandler<CreateCustomerOrderCommand, OrderResponse>
  {
    public async Task<OrderResponse> Handle(CreateCustomerOrderCommand request, CancellationToken cancellationToken)
    {
      return new OrderResponse() { Id = Guid.NewGuid(), HelloMessage = "Yooooo" };
    }
  }
}