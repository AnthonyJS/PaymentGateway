using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Commands
{
  public class CreatePaymentCommand : IRequest<Result<AcquiringBankPayment>>
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

  public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<AcquiringBankPayment>>
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

    public async Task<Result<AcquiringBankPayment>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
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
        return Result.Failure<AcquiringBankPayment>("Failed to save to the DB");
      }

      return Result.Ok(new AcquiringBankPayment()
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
