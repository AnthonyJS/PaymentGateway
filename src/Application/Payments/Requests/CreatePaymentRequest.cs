using System;

namespace PaymentGateway.Application.Requests
{
  public class CreatePaymentRequest
  {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public byte ExpiryMonth { get; set; }
    public byte ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string CVV { get; set; }
  }
}