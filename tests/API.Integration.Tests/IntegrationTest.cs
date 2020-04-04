using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using PaymentGateway.API;

namespace API.Integration.Tests
{
  public class IntegrationTest
  {
    protected readonly HttpClient TestClient;

    public IntegrationTest()
    {
      var appFactory = new WebApplicationFactory<Startup>();
      TestClient = appFactory.CreateClient();
    }
  }
}
