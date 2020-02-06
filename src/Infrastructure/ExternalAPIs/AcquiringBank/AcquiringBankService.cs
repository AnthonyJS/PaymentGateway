using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;

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