using System;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.SeedWork;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate
{
  public class Payment : Entity, IAggregateRoot
  {
    public CardDetails CardDetails { get; private set; }
    public Currency Currency { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public Guid? AcquiringBankId { get; private set; }
    public string ErrorMessage { get; private set; }

    private Payment() : base(Guid.Empty) { }
    
    public Payment(Guid id, CardDetails cardDetails, Currency currency, decimal amount, Guid acquiringBankId)
      : base(id)
    {
      CardDetails = cardDetails ?? throw new PaymentDomainException(nameof(cardDetails));
      Amount = amount > 0 ? amount : throw new PaymentDomainException(nameof(amount));
      Currency = currency;
      PaymentStatus = PaymentStatus.Initialized;
      AcquiringBankId = acquiringBankId;
    }

    public Payment(Guid id, CardDetails cardDetails, Currency currency, decimal amount)
      : this(id, cardDetails, currency, amount, Guid.Empty)
    {
    }

    public void SetSubmitting()
    {
      if (PaymentStatus == PaymentStatus.Initialized)
      {
        PaymentStatus = PaymentStatus.Submitting;
      }
    }
    
    public void SetSuccess(Guid acquiringBankId)
    {
      if (PaymentStatus == PaymentStatus.Submitting)
      {
        AcquiringBankId = acquiringBankId;
        PaymentStatus = PaymentStatus.Success;
      }
    }
    
    public void SetFailure(string error)
    {
      if (PaymentStatus == PaymentStatus.Submitting)
      {
        ErrorMessage = error;
        PaymentStatus = PaymentStatus.Failure;
      }
    }
  }
}
