using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  public class AcquiringBankGateway : IAcquiringBankGateway
  {
    private readonly IAcquiringBankHttpClient _acquiringBankHttpClient;

    public AcquiringBankGateway(IAcquiringBankHttpClient acquiringBankHttpClient)
    {
      _acquiringBankHttpClient = acquiringBankHttpClient;
    }
    public async Task<Result<Guid>> ProcessPayment(Payment payment)
    {
      return await _acquiringBankHttpClient.ProcessPayment(payment);

      // // TODO: Make this Result<Acquiring.....>
      // return new AcquiringBankResponse()
      // {
      //   Id = Guid.NewGuid(),
      //   Amount = request.Amount,
      //   // A simple case to be able to toggle success / failure
      //   StatusCode = request.Amount > 1 && request.Amount <= 10000
      //     ? AcquiringBankStatusCode.Success
      //     : AcquiringBankStatusCode.Error1
      // };
    }
  }
}