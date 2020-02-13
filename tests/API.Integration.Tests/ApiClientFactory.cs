using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.API.Integration.Tests
{
  public class ApiClientFactory : WebApplicationFactory<Startup>
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureTestServices(services =>
      {
        services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();
      });
    }
  }
}


