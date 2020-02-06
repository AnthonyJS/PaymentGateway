using System;
using System.Threading.Tasks;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Common.Interfaces;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  // TODO: The real implementation of this would have an HttpClient that makes call to the real bank.
  // This fake version just returns a success or failure based on the amount paid.
  public class AcquiringBankGateway : IAcquiringBankGateway
  {
    private readonly IAcquiringBankHttpClient _acquiringBankHttpClient;

    public AcquiringBankGateway(IAcquiringBankHttpClient acquiringBankHttpClient)
    {
      _acquiringBankHttpClient = acquiringBankHttpClient;
    }
    public async Task<AcquiringBankResponse> ProcessPayment(AcquiringBankRequest request)
    {
      // TODO: Make this Result<Acquiring.....>
      return new AcquiringBankResponse()
      {
        Id = Guid.NewGuid(),
        Amount = request.Amount,
        // A simple case to be able to toggle success / failure
        StatusCode = request.Amount > 1 && request.Amount <= 10000
          ? AcquiringBankStatusCode.Success
          : AcquiringBankStatusCode.Error1
      };
    }
  }
}