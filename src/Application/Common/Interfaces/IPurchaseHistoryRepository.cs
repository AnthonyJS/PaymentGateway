using System;
using System.Collections.Generic;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Application.Common.Interfaces
{
  public interface IPaymentHistoryRepository
  {
    void InsertPayment(Payment payment);
    Payment GetPaymentById(Guid id);
  }
}