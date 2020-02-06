using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.AcquiringBank.Models;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Application.Common.Interfaces
{
  public interface IAcquiringBankGateway
  {
    Task<Result<Guid>> ProcessPayment(Payment payment);
  }
}