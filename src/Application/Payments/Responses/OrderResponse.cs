using System;

namespace PaymentGateway.Application.Responses
{
  public class OrderResponse
  {
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string HelloMessage { get; set; }
  }
}