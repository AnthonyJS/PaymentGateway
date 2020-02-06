using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;
using PaymentGateway.Application.Queries;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Handlers
{
  public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentByIdResponse>
  {
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(IPaymentHistoryRepository paymentHistoryRepository, IMapper mapper)
    {
      _paymentHistoryRepository = paymentHistoryRepository;
      _mapper = mapper;
    }

    public async Task<PaymentByIdResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
      Result<Payment> result = await _paymentHistoryRepository.GetPaymentById(request.Id);
      Payment payment = result.Value;

      return _mapper.Map<PaymentByIdResponse>(payment);
    }
  }
}
