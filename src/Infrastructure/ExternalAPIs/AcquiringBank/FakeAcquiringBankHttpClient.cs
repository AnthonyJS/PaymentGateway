using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Common.Enums;
using PaymentGateway.Application.Common.Interface;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  // TODO: The real implementation of this would have an HttpClient that 
  // makes call to the real bank. This fake version just returns a
  //  success or failure based on the amount paid and currency.
  public class FakeAcquiringBankHttpClient : IAcquiringBankHttpClient
  {
    private readonly List<Currency> _permittedCurrencies = new List<Currency>()
             { Currency.GBP, Currency.EUR, Currency.USD };

    public async Task<Result<Guid>> ProcessPayment(Payment payment)
    {
      if (!_permittedCurrencies.Contains(payment.Currency))
        return Result.Failure<Guid>($"We do not currently accept {payment.Currency.ToString()}");

      if (payment.Amount == 0)
        return Result.Failure<Guid>("Amount must be greater than 0");

      if (payment.Amount >= 10000)
        return Result.Failure<Guid>("Amount must be less than £10,000");

      return Result.Ok(Guid.NewGuid());
    }
  }
}