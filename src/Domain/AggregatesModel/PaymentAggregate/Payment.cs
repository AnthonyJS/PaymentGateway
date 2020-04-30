using System;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.SeedWork;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate
{
  public class Payment : Entity, IAggregateRoot
  {
    public CardDetails CardDetails { get; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    
    public PaymentStatus PaymentStatus { get; private set; }
    public Guid AcquiringBankId { get; private set; }
    public string ErrorMessage { get; private set; }
    
    public Payment(Guid id, CardDetails cardDetails, Currency currency, decimal amount) 
      : base(id)
    {
      CardDetails = cardDetails ?? throw new PaymentDomainException(nameof(cardDetails));
      Currency = currency ?? throw new PaymentDomainException(nameof(currency));
      Amount = amount > 0 ? amount : throw new PaymentDomainException(nameof(amount));
      PaymentStatus = PaymentStatus.Initialized;
    }

    public void SetSubmitting()
    {
      if (PaymentStatus == PaymentStatus.Initialized)
      {
        PaymentStatus = PaymentStatus.Submitting;
        // TODO: Send Domain event here
      }
    }
    
    public void SetSuccess(Guid acquiringBankId)
    {
      if (PaymentStatus == PaymentStatus.Submitting)
      {
        AcquiringBankId = acquiringBankId;
        PaymentStatus = PaymentStatus.Success;
        // TODO: Send Domain event here
      }
    }
    
    public void SetFailure(string error)
    {
      if (PaymentStatus == PaymentStatus.Submitting)
      {
        ErrorMessage = error;
        PaymentStatus = PaymentStatus.Failure;
        // TODO: Send Domain event here
      }
    }
  }
}
