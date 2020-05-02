using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Metrics;

namespace PaymentGateway.API.Application.Queries
{
  public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<GetPaymentByIdResponse>>
  {
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMetrics _metrics;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(IPaymentHistoryRepository paymentHistoryRepository, IMetrics metrics, IMapper mapper)
    {
      _paymentHistoryRepository = paymentHistoryRepository;
      _metrics = metrics;
      _mapper = mapper;
    }

    public async Task<Result<GetPaymentByIdResponse>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
      Result<Payment> result = await _paymentHistoryRepository.GetPaymentById(request.Id);

      if (result.IsFailure)
        return Result.Failure<GetPaymentByIdResponse>(GetPaymentErrors.PaymentNotFound);

      Payment payment = result.Value;
      
      _metrics.Measure.Counter.Increment(MetricsRegistry.PaymentsRetrievedCounter);

      return Result.Ok(_mapper.Map<GetPaymentByIdResponse>(payment));
    }
  }
  
  public static class GetPaymentErrors
  {
    public static readonly string PaymentNotFound = "Unable to find payment";
  }
}
