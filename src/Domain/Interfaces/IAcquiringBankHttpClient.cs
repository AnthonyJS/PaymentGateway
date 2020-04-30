using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Domain.Interfaces
{
  public interface IAcquiringBankHttpClient
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}
