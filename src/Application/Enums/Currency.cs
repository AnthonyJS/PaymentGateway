using System.ComponentModel;

namespace PaymentGateway.Application.Enums
{
  public enum Currency
  {
    [Description("GBP")]
    GBP = 1,
    [Description("USD")]
    USD,
    [Description("EUR")]
    EUR,
    [Description("AUD")]
    AUD,
    [Description("JPY")]
    JPY
  }
}