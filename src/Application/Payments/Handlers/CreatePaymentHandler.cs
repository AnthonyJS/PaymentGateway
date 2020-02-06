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
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentHandler(IAcquiringBank acquiringBank, IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _acquiringBank = acquiringBank;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<PaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
      var acquiringBankRequest = _mapper.Map<AcquiringBankRequest>(request);

      AcquiringBankResponse result = await _acquiringBank.ProcessPayment(acquiringBankRequest);

      // TODO: If (result.IsSuccess)

      var payment = _mapper.Map<Payment>(request);
      payment.Id = Guid.NewGuid();
      payment.AcquiringBankId = result.Id;

      _paymentHistoryRepository.InsertPayment(payment);

      return new PaymentResponse()
      {
        Id = payment.Id,
        StatusMessage = result.StatusCode == AcquiringBankStatusCode.Success
          ? "Succccceessss"
          : "Booo"
      };
    }
  }
}