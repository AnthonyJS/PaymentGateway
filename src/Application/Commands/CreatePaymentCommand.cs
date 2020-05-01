using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;

namespace PaymentGateway.Application.Commands
{
  public class CreatePaymentCommand : IRequest<Result<Payment>>
  {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public byte ExpiryMonth { get; set; }
    public byte ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
  }

  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
  {
    private readonly IAcquiringBankService _acquiringBankService;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMetrics _metrics;
    private readonly ILogger<CreatePaymentCommandHandler> _logger;
    private readonly IEventStoreClient _eventStoreClient;
    private IDomainEvent _domainEvent;

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService,
      IPaymentHistoryRepository paymentHistoryRepository, IMetrics metrics, 
      ILogger<CreatePaymentCommandHandler> logger, IEventStoreClient eventStoreClient)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _metrics = metrics;
      _logger = logger;
      _eventStoreClient = eventStoreClient;
    }

    // TODO: Make whole thing atomic with Unit of Work
    public async Task<Result<Payment>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
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
      
      if (dbResult.IsFailure || eventStoreResult.IsFailure)
        return Result.Failure<Payment>("Failed to save Payment");
      
      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsCreatedCounter);

      return acquiringBankResult.IsSuccess  
        ? Result.Ok(payment)
        : Result.Failure<Payment>("Acquiring bank refused payment");
    }
  }
}
