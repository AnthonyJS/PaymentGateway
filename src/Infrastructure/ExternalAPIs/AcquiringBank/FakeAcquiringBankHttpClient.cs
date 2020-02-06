using System;
using System.Threading.Tasks;
using PaymentGateway.Application.AcquiringBank.Enums;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Common.Interfaces;

namespace PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank
{
  // TODO: The real implementation of this would have an HttpClient that makes call to the real bank.
  // This fake version just returns a success or failure based on the amount paid.
  public class FakeAcquiringBankHttpClient : IAcquiringBankHttpClient
  {

  }
}