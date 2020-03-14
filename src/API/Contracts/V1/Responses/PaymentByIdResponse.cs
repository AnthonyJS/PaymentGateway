using System;

namespace PaymentGateway.API.Contracts.V1.Responses
{
  public class PaymentByIdResponse
  {
    public Guid Id { get; set; }
    public Guid AcquiringBankId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumberMasked { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
  }
}
