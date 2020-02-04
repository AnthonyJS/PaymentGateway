using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
  {
    private readonly IPurchaseHistoryRepository _purchaseHistoryRepository;

    public GetOrderByIdHandler(IPurchaseHistoryRepository purchaseHistoryRepository)
    {
      _purchaseHistoryRepository = purchaseHistoryRepository;
    }

    public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
      Purchase purchase = _purchaseHistoryRepository.GetPurchaseById(request.Id);

      return new OrderResponse()
      {
        Id = purchase.Id,
        Amount = purchase.Amount
      };
    }
  }
}
