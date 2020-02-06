using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  // TODO: The real implementation of this would have an HttpClient that makes call to the real bank.
  // This fake version just returns a success or failure based on the amount paid.
  public class FakeAcquiringBankHttpClient : IAcquiringBankHttpClient
  {
    public async Task<Result<Guid>> ProcessPayment(Payment payment)
    {
      if (payment.Amount > 0 && payment.Amount <= 10000)
        return Result.Ok(Guid.NewGuid());

      return Result.Failure<Guid>("Summat went wrong");
    }
  }
}