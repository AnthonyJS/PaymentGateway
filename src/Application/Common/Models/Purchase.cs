using PaymentGateway.Application.Common.Enums;

namespace PaymentGateway.Application.Common.Models
{
  public class Purchase
  {
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    // TODO: Should send this as a string rather than an arbitrary Id
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public string CVV { get; set; }
  }
}