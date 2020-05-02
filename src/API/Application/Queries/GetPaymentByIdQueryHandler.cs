using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;

namespace PaymentGateway.API.Application.Queries
{
  public class GetPaymentByIdQuery : IRequest<Result<Payment>>
  {
    public Guid Id { get; set; }

    public GetPaymentByIdQuery(Guid id)
    {
      Id = id;
    }
  }
  
  public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<Payment>>
  {
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMetrics _metrics;

    public GetPaymentByIdQueryHandler(IPaymentHistoryRepository paymentHistoryRepository, IMetrics metrics)
    {
      _paymentHistoryRepository = paymentHistoryRepository;
      _metrics = metrics;
    }

    public async Task<Result<Payment>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
      Result<Payment> result = await _paymentHistoryRepository.GetPaymentById(request.Id);

      if (result.IsFailure)
        return Result.Failure<Payment>(GetPaymentErrors.PaymentNotFound);

      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsRetrievedCounter);

      Payment payment = result.Value;
      
      return Result.Ok(payment);
    }
  }
  
  public static class GetPaymentErrors
  {
    public static readonly string PaymentNotFound = "Unable to find payment";
  }
}
