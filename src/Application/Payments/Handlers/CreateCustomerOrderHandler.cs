using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Common.Enums;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class CreateCustomerOrderHandler : IRequestHandler<CreateCustomerOrderCommand, OrderResponse>
  {
    private readonly IAcquiringBank _acquiringBank;
    private readonly IPurchaseHistoryRepository _purchaseHistoryRepository;
    public CreateCustomerOrderHandler(IAcquiringBank acquiringBank, IPurchaseHistoryRepository purchaseHistoryRepository)
    {
      _acquiringBank = acquiringBank;
      _purchaseHistoryRepository = purchaseHistoryRepository;
    }

    public async Task<OrderResponse> Handle(CreateCustomerOrderCommand request, CancellationToken cancellationToken)
    {
      var acquiringBankRequest = new AcquiringBankRequest()
      {
        Amount = request.Amount
      };

      AcquiringBankResponse result = await _acquiringBank.ProcessPayment(acquiringBankRequest);

      var purchase = new Purchase()
      {
        Id = result.Id,
        // CardNumber = "123456",
        Amount = request.Amount,
        Currency = Currency.GBP
      };

      _purchaseHistoryRepository.InsertPurchase(purchase);

      return new OrderResponse()
      {
        Id = result.Id,
        Amount = result.Amount,
        HelloMessage = result.StatusCode == AcquiringBankStatusCode.Success
          ? "Succccceessss"
          : "Booo"
      };
    }
  }
}