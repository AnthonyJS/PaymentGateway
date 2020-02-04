using System;
using System.Collections.Generic;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Infrastructure.Persistence.PurchaseHistory
{
  public class PurchaseHistoryRepository : IPurchaseHistoryRepository
  {
    public List<Purchase> GetPurchaseById(Guid id)
    {
      throw new NotImplementedException();
    }

    public void InsertPurchase(Purchase purchase)
    {
      throw new NotImplementedException();
    }
  }
}