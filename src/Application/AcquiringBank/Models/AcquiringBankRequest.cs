using PaymentGateway.Application.Common.Enums;

namespace PaymentGateway.Application.AcquiringBank.Models
{
  public class AcquiringBankRequest
  {
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string CVV { get; set; }
  }
}