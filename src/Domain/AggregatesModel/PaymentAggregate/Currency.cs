using PaymentGateway.Domain.SeedWork;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate
{
  public class Currency : Enumeration
  {
    public static Currency GBP = new Currency(1, nameof(GBP));    
    public static Currency USD = new Currency(1, nameof(USD));    
    public static Currency EUR = new Currency(1, nameof(EUR));    
    public static Currency AUD = new Currency(1, nameof(AUD));    
    public static Currency JPY = new Currency(1, nameof(JPY));
    
    public Currency(int id, string name)
      : base(id, name)
    {
    }
  }
}
