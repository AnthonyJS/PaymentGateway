using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Handlers
{
  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
  {
    private readonly IAcquiringBankService _acquiringBankService;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService, IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
      Payment payment = _mapper.Map<Payment>(request);

      // TODO: The payment request to the acquiring bank and save the result
      // to the DB could be done in an atomic transaction.
      Result<Guid> result = await _acquiringBankService.ProcessPayment(payment);

      if (result.IsFailure)
        return Result.Failure<Guid>($"The acquirer bank would not process the payment: {result.Error}");

      payment.Id = Guid.NewGuid();
      payment.AcquiringBankId = result.Value;

      Result dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (dbResult.IsFailure)
      {
        // TODO: Rollback payment with bank here
        return Result.Failure<Guid>("Failed to save to the DB");
      }

      return Result.Ok(payment.Id);
    }
  }
}