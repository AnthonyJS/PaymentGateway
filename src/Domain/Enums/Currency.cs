using System.ComponentModel;

namespace PaymentGateway.Domain.Enums
{
  public enum Currency
  {
    [Description(nameof(GBP))]
    GBP = 1,
    [Description(nameof(USD))]
    USD,
    [Description(nameof(EUR))]
    EUR,
    [Description(nameof(AUD))]
    AUD,
    [Description(nameof(JPY))]
    JPY
  }
}
