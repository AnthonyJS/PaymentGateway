using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Application.Common.Interfaces
{
  public interface IAcquiringBankService
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}