﻿using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Metrics;
using PaymentGateway.Application.Models;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Application.Queries
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
        return Result.Failure<Payment>("Unable to find payment");

      Payment payment = result.Value;
      
      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsRetrievedCounter);

      return Result.Ok(payment);
    }
  }
}


