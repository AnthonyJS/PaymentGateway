using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;
using Xunit;

namespace PaymentGateway.API.Integration.Tests
{
  public class PaymentTestFixture : IAsyncLifetime
  {
    public IPaymentHistoryRepository PaymentHistoryRepository { get; }
    public HttpClient TestClient { get; }

    private DockerTestContainer _dockerTestContainer;

    public PaymentTestFixture()
    {
      PaymentHistoryRepository = new PaymentHistoryRepository(null, null);
      var appFactory = new WebApplicationFactory<Startup>();
      TestClient = appFactory.CreateClient();
    }

    public async Task InitializeAsync()
    {
      _dockerTestContainer = new DockerTestContainer();
        
      await _dockerTestContainer.CreateContainer();
      await _dockerTestContainer.StartContainer();
    }

    public async Task DisposeAsync()
    {
      await _dockerTestContainer.DeleteContainer();
    }
  }
}
