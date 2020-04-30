using PaymentGateway.Domain.SeedWork;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate
{
  public class PaymentStatus : Enumeration
  {
    
    public static PaymentStatus Initialized = new PaymentStatus(1, nameof(Initialized));  
    public static PaymentStatus Submitting = new PaymentStatus(1, nameof(Submitting));  
    public static PaymentStatus Success = new PaymentStatus(1, nameof(Success));  
    public static PaymentStatus Failure = new PaymentStatus(1, nameof(Failure));  
    
    public PaymentStatus(int id, string name) : base(id, name)
    {
    }
  }
}
