using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
  {
    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
      return new OrderResponse() { HelloMessage = "howdy" };
    }
  }
}
