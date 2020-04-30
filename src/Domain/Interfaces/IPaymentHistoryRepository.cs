using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.Domain.Interfaces
{
  public interface IPaymentHistoryRepository
  {
    Task<Result> InsertPayment(Payment payment);
    Task<Result<Payment>> GetPaymentById(Guid id);
  }
}
