using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponse>>
  {
    public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
      return new List<OrderResponse>(){
        new OrderResponse() { HelloMessage = "howdy", Id = Guid.NewGuid() },
        new OrderResponse() { HelloMessage = "boomzilla", Id = Guid.NewGuid()  }
      };
    }
  }
}