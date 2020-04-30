using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Models;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Application.Interfaces
{
  public interface IAcquiringBankService
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}
