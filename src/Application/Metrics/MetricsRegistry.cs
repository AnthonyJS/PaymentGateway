using App.Metrics;
using App.Metrics.Counter;

namespace PaymentGateway.Application.Metrics
{
  public class MetricsRegistry
  {
    public static CounterOptions PaymentsCreatedCounter => new CounterOptions
    {
      Name = "Created Payments", Context = "PaymentsApi", MeasurementUnit = Unit.Calls
    };
    
    public static CounterOptions PaymentsRetrievedCounter => new CounterOptions
    {
      Name = "Payments retrieved", Context = "PaymentsApi", MeasurementUnit = Unit.Calls
    };
  }
}
