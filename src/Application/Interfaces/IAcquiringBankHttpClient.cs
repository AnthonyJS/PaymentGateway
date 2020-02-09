using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Interfaces
{
  public interface IAcquiringBankHttpClient
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}