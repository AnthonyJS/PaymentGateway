using System;

namespace PaymentGateway.Application.Responses
{
  public class PaymentResponse
  {
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string HelloMessage { get; set; }
  }
}