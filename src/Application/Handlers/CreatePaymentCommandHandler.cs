﻿using System;
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
  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<AcquiringBankDto>>
  {
    private readonly IAcquiringBankService _acquiringBankService;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService,
      IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<Result<AcquiringBankDto>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
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
        return Result.Failure<AcquiringBankDto>("Failed to save to the DB");
      }

      return Result.Ok(new AcquiringBankDto()
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
