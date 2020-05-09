using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.API.Filters;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank;
using PaymentGateway.Infrastructure.Persistence.EventStore;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;
using PaymentGateway.Infrastructure.Security;

namespace PaymentGateway.API.Core
{
  public static class ConfigureDependencyInjectionExtensions
  {
    public static IServiceCollection AddDependencyInjectionWireup(this IServiceCollection services)
    {
      services.AddScoped<IAcquiringBankService, AcquiringBankService>();
      services.AddScoped<IAcquiringBankHttpClient, FakeAcquiringBankHttpClient>();
      services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();
      services.AddScoped<IApiAuthorizationService, ApiAuthorizationService>();
      services.AddScoped<FullCardAccessAttribute>();
      
      services.AddSingleton<IEventStoreClient, EventStoreClient>();
      
      return services;
    }
  }
}
