using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;

namespace PaymentGateway.API.Core
{
  public static class ConfigureDependencyInjectionExtensions
  {
    public static IServiceCollection AddDependencyInjectionWireup(this IServiceCollection services)
    {
      services.AddScoped<IAcquiringBankService, AcquiringBankService>();
      services.AddScoped<IAcquiringBankHttpClient, FakeAcquiringBankHttpClient>();
      services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();

      return services;
    }
  }
}
