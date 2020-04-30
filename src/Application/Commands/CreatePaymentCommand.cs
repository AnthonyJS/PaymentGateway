using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Metrics;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.SeedWork;

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

    public CreatePaymentCommandHandler(IAcquiringBankService acquiringBankService,
      IPaymentHistoryRepository paymentHistoryRepository, IMetrics metrics, ILogger<CreatePaymentCommandHandler> logger)
    {
      _acquiringBankService = acquiringBankService;
      _paymentHistoryRepository = paymentHistoryRepository;
      _metrics = metrics;
      _logger = logger;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
      var cardDetails = new CardDetails(
        command.FirstName, 
        command.Surname, 
        command.CardNumber, 
        command.ExpiryMonth, 
        command.ExpiryYear, 
        command.CVV);
      // var currency = Enumeration.FromDisplayName<Currency>(command.Currency);
      var currency = Enum.Parse<Currency>(command.Currency);
      var payment = new Payment(Guid.NewGuid(), cardDetails, currency, command.Amount);
      
      payment.SetSubmitting();
      Result<Guid> acquiringBankResult = await _acquiringBankService.ProcessPayment(payment);

      if (acquiringBankResult.IsSuccess)
      {
        // TODO: Use structured logging
        _logger.LogInformation($"Acquiring bank processed payment {payment.Id} successfully");
        payment.SetSuccess(acquiringBankResult.Value);
      }
      else
      {
        _logger.LogWarning($"Acquiring bank would not process {payment.Id} {acquiringBankResult.Error}");
        payment.SetFailure(acquiringBankResult.Error);
      }

      Result dbResult = await _paymentHistoryRepository.InsertPayment(payment);

      if (dbResult.IsFailure)
      {
        // TODO: Make atomic
        _logger.LogError($"Failed to save the Payment {payment.Id} to the DB");
        return Result.Failure<Payment>("Failed to save to the DB");
      }
      
      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsCreatedCounter);

      return Result.Ok(payment);
    }
  }
}
