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
  public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentResponse>
  {
    private readonly IPurchaseHistoryRepository _purchaseHistoryRepository;

    public GetPaymentByIdHandler(IPurchaseHistoryRepository purchaseHistoryRepository)
    {
      _purchaseHistoryRepository = purchaseHistoryRepository;
    }

    public async Task<PaymentResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
      Purchase purchase = _purchaseHistoryRepository.GetPurchaseById(request.Id);

      return new PaymentResponse()
      {
        Id = purchase.Id,
        Amount = purchase.Amount
      };
    }
  }
}
