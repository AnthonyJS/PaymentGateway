using System;
using System.Collections.Generic;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Application.Common.Interfaces
{
  public interface IPurchaseHistoryRepository
  {
    void InsertPurchase(Purchase purchase);
    List<Purchase> GetPurchaseById(Guid id);
  }
}