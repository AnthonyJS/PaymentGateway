using System;

namespace PaymentGateway.Application.Responses
{
  public class PaymentByIdResponse
  {
    public Guid Id { get; set; }
    public Guid AcquiringBankId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber4Digits { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string CVV { get; set; }
  }
}