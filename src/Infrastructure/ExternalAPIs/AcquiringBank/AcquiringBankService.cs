using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  public class AcquiringBankService : IAcquiringBankService
  {
    private readonly IAcquiringBankHttpClient _acquiringBankHttpClient;

    public AcquiringBankService(IAcquiringBankHttpClient acquiringBankHttpClient)
    {
      _acquiringBankHttpClient = acquiringBankHttpClient;
    }
    public async Task<Result<Guid>> ProcessPayment(Payment payment)
    {
      return await _acquiringBankHttpClient.ProcessPayment(payment);
    }
  }
}
