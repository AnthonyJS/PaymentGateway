using System;
using System.Collections.Generic;
using LiteDB;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Application.Common.Models;

namespace PaymentGateway.Infrastructure.Persistence.PurchaseHistory
{
  public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string[] Phones { get; set; }
    public bool IsActive { get; set; }
  }


  public class PurchaseHistoryRepository : IPurchaseHistoryRepository
  {
    public Purchase GetPurchaseById(Guid id)
    {
      using (var db = new LiteDatabase(@"MyData.db"))
      {
        var col = db.GetCollection<Purchase>("purchases");

        return col.FindOne(x => x.Id == id);
      }
    }

    public void InsertPurchase(Purchase purchase)
    {
      using (var db = new LiteDatabase(@"MyData.db"))
      {
        var col = db.GetCollection<Purchase>("purchases");

        col.Insert(purchase);
      }
    }
  }
}
