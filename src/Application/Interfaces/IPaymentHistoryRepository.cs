using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Interfaces
{
  public interface IPaymentHistoryRepository
  {
    Task<Result> InsertPayment(Payment payment);
    Task<Result<Payment>> GetPaymentById(Guid id);
  }
}