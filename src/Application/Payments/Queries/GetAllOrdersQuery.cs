using System.Collections.Generic;
using MediatR;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Queries
{
  public class GetAllOrdersQuery : IRequest<List<OrderResponse>>
  {

  }
}