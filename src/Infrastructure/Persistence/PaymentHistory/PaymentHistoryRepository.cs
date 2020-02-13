using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiteDB;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Infrastructure.Persistence.PaymentHistory
{
  public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string[] Phones { get; set; }
    public bool IsActive { get; set; }
  }


  public class PaymentHistoryRepository : IPaymentHistoryRepository
  {
    public async Task<Result<Payment>> GetPaymentById(Guid id)
    {
      using (var db = new LiteDatabase(@"MyData.db"))
      {
        var col = await Task.Run(() => db.GetCollection<Payment>("payments"));

        var result = col.FindOne(x => x.Id == id);

        if (result == null)
            return Result.Failure<Payment>("Payment not in DB");
          
        return Result.Ok(result);
      }
    }

    public async Task<Result> InsertPayment(Payment payment)
    {
      using (var db = new LiteDatabase(@"MyData.db"))
      {
        var col = await Task.Run(() => db.GetCollection<Payment>("payments"));

        col.Insert(payment);
      }

      return Result.Ok();
    }
  }
}
