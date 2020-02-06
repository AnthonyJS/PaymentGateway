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
    private readonly IAcquiringBankGateway _acquiringBank;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentHandler(IAcquiringBankGateway acquiringBank, IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _acquiringBank = acquiringBank;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<PaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {

      var payment = _mapper.Map<Payment>(request);

      // var acquiringBankRequest = _mapper.Map<AcquiringBankRequest>(request);

      var result = await _acquiringBank.ProcessPayment(payment);

      if (result.IsFailure)
        return new PaymentResponse()
        {
          // Id = payment.Id,
          StatusMessage = "Booo"
        };

      payment.Id = Guid.NewGuid();
      payment.AcquiringBankId = result.Value;

      _paymentHistoryRepository.InsertPayment(payment);

      return new PaymentResponse()
      {
        Id = payment.Id,
        StatusMessage = result.IsSuccess
          ? "Succccceessss"
          : "Booo"
      };
    }
  }
}