using System.Text.Json;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Events
{
  public class PaymentFailedDomainEvent : IDomainEvent
  {
    public string JSON { get; }

    public PaymentFailedDomainEvent(Payment payment)
    {
      JSON = JsonSerializer.Serialize(payment);
    }   
  }
}
