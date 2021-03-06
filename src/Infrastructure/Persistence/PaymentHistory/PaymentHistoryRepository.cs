using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using LiteDB;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Infrastructure.Persistence.PaymentHistory
{
  public class PaymentHistoryRepository : IPaymentHistoryRepository
  {
    private readonly ILogger<PaymentHistoryRepository> _logger;
    private readonly IMapper _mapper;

    public PaymentHistoryRepository(ILogger<PaymentHistoryRepository> logger, IMapper mapper)
    {
      _logger = logger;
      _mapper = mapper;
    }
    
    public async Task<Result<Payment>> GetPaymentById(Guid id)
    {
      try
      {
        using (var db = new LiteDatabase(@"MyData.db"))
        {
          var col = await Task.Run(() => db.GetCollection<PaymentDTO>("payments"));

          PaymentDTO result = col.FindById(id);

          if (result == null)
            return Result.Failure<Payment>(PaymentRepositoryErrors.PaymentNotInDb);

          var payment = _mapper.Map<Payment>(result);
          
          return Result.Ok(payment);
        }
      }
      catch (Exception e)
      {
        _logger.LogError($":( {e}");
      }
      
      return Result.Failure<Payment>(PaymentRepositoryErrors.PaymentRetrievalFailed);
    }

    public async Task<Result> InsertPayment(Payment payment)
    {
      using (var db = new LiteDatabase(@"MyData.db"))
      {
        var col = await Task.Run(() => db.GetCollection<PaymentDTO>("payments"));

        var paymentDto = new PaymentDTO()
        {
          Id = payment.Id,
          AcquiringBankId = payment.AcquiringBankId.Value,
          FirstName = payment.CardDetails.FirstName,
          Surname = payment.CardDetails.Surname,
          CardNumber = payment.CardDetails.CardNumber,
          ExpiryMonth = payment.CardDetails.ExpiryMonth,
          ExpiryYear = payment.CardDetails.ExpiryYear,
          CVV = payment.CardDetails.CVV,
          Currency = payment.Currency.ToString(),
          Amount = payment.Amount,
          PaymentStatusId = payment.PaymentStatus.Id
        };
        
        col.Insert(paymentDto);
      }

      return Result.Ok();
    }
  }
  
  public static class PaymentRepositoryErrors
  {
    public static readonly string PaymentNotInDb = "Payment not in DB";
    public static readonly string PaymentRetrievalFailed = "Payment retrieval failed";
  }
}
