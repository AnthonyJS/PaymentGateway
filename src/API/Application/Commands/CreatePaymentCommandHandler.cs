﻿using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;

namespace PaymentGateway.API.Application.Commands
{
  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<CreatePaymentSuccessResponse>>
  {
    private readonly IAcquiringBankService _acquiringBankService;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMetrics _metrics;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IEventStoreClient _eventStoreClient;
    private readonly IMapper _mapper;
    private IDomainEvent _domainEvent;

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService,
      IPaymentHistoryRepository paymentHistoryRepository, IMetrics metrics, 
      ILogger<CreatePaymentCommandHandler> logger, IEventStoreClient eventStoreClient, IMapper mapper)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _metrics = metrics;
      _logger = logger;
      _eventStoreClient = eventStoreClient;
      _mapper = mapper;
    }

    // TODO: Make whole thing atomic with Unit of Work
    public async Task<Result<CreatePaymentSuccessResponse>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
      var cardDetails = new CardDetails(command.FirstName, command.Surname, command.CardNumber, 
        command.ExpiryMonth, command.ExpiryYear, command.CVV);
      var currency = Enum.Parse<Currency>(command.Currency);
      var payment = new Payment(Guid.NewGuid(), cardDetails, currency, command.Amount);
      
      payment.SetSubmitting();
      Result<Guid> acquiringBankResult = await _acquiringBankService.ProcessPayment(payment);
      
      if (acquiringBankResult.IsSuccess)
      {
        // TODO: Use structured logging
        _logger.LogInformation($"Acquiring bank processed payment {payment.Id} successfully");
        payment.SetSuccess(acquiringBankResult.Value);
        _domainEvent = new PaymentSuccessfulDomainEvent(payment);
      }
      else
      {
        _logger.LogWarning($"Acquiring bank would not process {payment.Id} {acquiringBankResult.Error}");
        payment.SetFailure(acquiringBankResult.Error);
        _domainEvent = new PaymentFailedDomainEvent(payment);
      }
      
      Result dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (dbResult.IsFailure)
      {
        _logger.LogError($"Failed to save the Payment {payment.Id} to the DB");
      }
      
      var eventStoreResult = await _eventStoreClient.Write(_domainEvent);
      
      if (eventStoreResult.IsFailure)
      {
        _logger.LogError($"Failed to send the Domain Event for Payment {payment.Id} of type {_domainEvent.GetType()}");
      }
      
      if (dbResult.IsFailure)
        return Result.Failure<CreatePaymentSuccessResponse>(CreatePaymentErrors.PaymentSaveFailed);
      
      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsCreatedCounter);
      
      var response = _mapper.Map<CreatePaymentSuccessResponse>(payment);

      return acquiringBankResult.IsSuccess  
        ? Result.Ok(response)
        : Result.Failure<CreatePaymentSuccessResponse>(CreatePaymentErrors.AcquiringBankRefusedPayment);
    }
  }
  
  public static class CreatePaymentErrors
  {
    public static readonly string PaymentSaveFailed = "Failed to save Payment";
    public static readonly string AcquiringBankRefusedPayment = "Acquiring bank refused payment";
  }
}
