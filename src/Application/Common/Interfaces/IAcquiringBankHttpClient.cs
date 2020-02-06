using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Application.Common.Interface
{
  public interface IAcquiringBankHttpClient
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}