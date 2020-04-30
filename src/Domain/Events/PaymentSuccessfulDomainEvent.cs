using System.Text.Json;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Events
{
  public class PaymentSuccessfulDomainEvent : IDomainEvent
  {
    public string JSON { get; }

    public PaymentSuccessfulDomainEvent(Payment payment)
    {
      JSON = JsonSerializer.Serialize(payment);
    }   
  }
}
