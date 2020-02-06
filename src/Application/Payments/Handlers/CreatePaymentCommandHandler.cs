using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
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
  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
  {
    private readonly IAcquiringBankGateway _acquiringBank;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandler(IAcquiringBankGateway acquiringBank, IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _acquiringBank = acquiringBank;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {

      Payment payment = _mapper.Map<Payment>(request);

      // TODO: Could put this in an atomic transaction

      Result<Guid> result = await _acquiringBank.ProcessPayment(payment);

      if (result.IsFailure)
        return Result.Failure<Guid>($"The acquirer bank would not process the payment: {result.Error}");

      payment.Id = Guid.NewGuid();
      payment.AcquiringBankId = result.Value;

      var dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (result.IsFailure)
        return Result.Failure<Guid>("Failed to save to the DB");

      return Result.Ok(payment.Id);
    }
  }
}