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
  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
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

    public async Task<Result<PaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
      Payment payment = _mapper.Map<Payment>(request);

      // TODO: The payment request to the acquiring bank and save the result
      // to the DB could be done in an atomic transaction.
      Result<Guid> acquiringBankResult = await _acquiringBankService.ProcessPayment(payment);

      // if (result.IsFailure)
      //   return Result.Failure<PaymentResponse>($"The acquirer bank would not process the payment: {result.Error}");

      payment.Id = Guid.NewGuid();
      payment.IsSuccess = acquiringBankResult.IsSuccess;

      if (payment.IsSuccess)
      {
        payment.AcquiringBankId = acquiringBankResult.Value;
      }
      else
      {
        payment.ErrorMessage = acquiringBankResult.Error;
      }

      Result dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (dbResult.IsFailure)
      {
        // TODO: Rollback payment with bank here
        return Result.Failure<PaymentResponse>("Failed to save to the DB");
      }

      return Result.Ok(new PaymentResponse()
      {
        Id = payment.Id,
        IsSuccess = acquiringBankResult.IsSuccess,
        ErrorMessage = acquiringBankResult.IsSuccess ? string.Empty : acquiringBankResult.Error
      });
    }
  }
}