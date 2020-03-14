using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Queries;

namespace PaymentGateway.Application.Handlers
{
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
