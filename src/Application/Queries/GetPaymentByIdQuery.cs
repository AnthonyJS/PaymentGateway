using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Models;

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

    public GetPaymentByIdQueryHandler(IPaymentHistoryRepository paymentHistoryRepository)
    {
      _paymentHistoryRepository = paymentHistoryRepository;
    }

    public async Task<Result<Payment>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
      Result<Payment> result = await _paymentHistoryRepository.GetPaymentById(request.Id);

      if (result.IsFailure)
        return Result.Failure<Payment>("Unable to find payment");

      Payment payment = result.Value;

      return Result.Ok(payment);
    }
  }
}


