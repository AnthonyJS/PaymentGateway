using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
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
    private readonly IValidator<CreatePaymentCommand> _validator;

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService,
      IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper,
      IValidator<CreatePaymentCommand> validator)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
      _validator = validator;
    }

    public async Task<Result<PaymentResponse>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
      var validationResult = _validator.Validate(command);

      if (!validationResult.IsValid)
        return Result.Failure<PaymentResponse>(
            string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

      Payment payment = _mapper.Map<Payment>(command);

      // TODO: The payment request to the acquiring bank and saving the result
      // to the DB should be done in an atomic transaction, so a rollback
      // can be performed in case one part fails.
      Result<Guid> acquiringBankResult = await _acquiringBankService.ProcessPayment(payment);

      payment.Id = Guid.NewGuid();
      payment.IsSuccess = acquiringBankResult.IsSuccess;

      if (payment.IsSuccess)
        payment.AcquiringBankId = acquiringBankResult.Value;
      else
        payment.ErrorMessage = acquiringBankResult.Error;

      Result dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (dbResult.IsFailure)
      {
        return Result.Failure<PaymentResponse>("Failed to save to the DB");
      }

      return Result.Ok(new PaymentResponse()
      {
        Id = payment.Id,
        IsSuccess = acquiringBankResult.IsSuccess,
        ErrorMessage = acquiringBankResult.IsSuccess
                        ? string.Empty
                        : acquiringBankResult.Error
      });
    }
  }
}