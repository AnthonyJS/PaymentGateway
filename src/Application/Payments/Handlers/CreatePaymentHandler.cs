using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
  public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentResponse>
  {
    private readonly IAcquiringBank _acquiringBank;
    private readonly IPurchaseHistoryRepository _purchaseHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentHandler(IAcquiringBank acquiringBank, IPurchaseHistoryRepository purchaseHistoryRepository, IMapper mapper)
    {
      _acquiringBank = acquiringBank;
      _purchaseHistoryRepository = purchaseHistoryRepository;
      _mapper = mapper;
    }

    public async Task<PaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
      var acquiringBankRequest = _mapper.Map<AcquiringBankRequest>(request);

      AcquiringBankResponse result = await _acquiringBank.ProcessPayment(acquiringBankRequest);

      var purchase = new Purchase()
      {
        Id = result.Id,
        // CardNumber = "123456",
        Amount = request.Amount,
        Currency = Currency.GBP
      };

      _purchaseHistoryRepository.InsertPurchase(purchase);

      return new PaymentResponse()
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