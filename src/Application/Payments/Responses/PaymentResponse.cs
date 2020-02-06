using System;

namespace PaymentGateway.Application.Responses
{
  public class PaymentResponse
  {
    public Guid Id { get; set; }
    public string StatusMessage { get; set; }
  }
}