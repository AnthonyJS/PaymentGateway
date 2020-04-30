using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Domain.Interfaces
{
  public interface IAcquiringBankService
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}
