using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;

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
      return await Task.Run(() => DoFakeValidation(payment));
    }

    private Result<Guid> DoFakeValidation(Payment payment)
    {
      if (!_permittedCurrencies.Contains(payment.Currency))
        return Result.Failure<Guid>(string.Format(FakeAcquiringBankErrors.InvalidCurrency, payment.Currency));

      if (payment.Amount == 0)
        return Result.Failure<Guid>(FakeAcquiringBankErrors.AmountMustBeGreaterThan0);

      if (payment.Amount >= 10000)
        return Result.Failure<Guid>(FakeAcquiringBankErrors.AmountMustBeLessThan10000);

      return Result.Ok(Guid.NewGuid());
    }
  }
  
  public static class FakeAcquiringBankErrors
  {
    public static readonly string AmountMustBeGreaterThan0 = "Amount must be greater than 0";
    public static readonly string AmountMustBeLessThan10000 = "Amount must be less than £10,000";
    public static readonly string InvalidCurrency = "We do not currently accept {0}";
  }
}
