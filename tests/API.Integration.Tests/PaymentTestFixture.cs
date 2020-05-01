using System.Net.Http;
using System.Threading;
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

    private EventStoreTestContainer _eventStoreTestContainer;

    public PaymentTestFixture()
    {
      PaymentHistoryRepository = new PaymentHistoryRepository(null, null);
      var appFactory = new WebApplicationFactory<Startup>();
      TestClient = appFactory.CreateClient();
    }

    public async Task InitializeAsync()
    {
      _eventStoreTestContainer = new EventStoreTestContainer();
        
      await _eventStoreTestContainer.CreateContainer();
      await _eventStoreTestContainer.StartContainer();
      
      // Give EventStore container enough time to start up 😬
      // TODO: Find a better way to do this
      Thread.Sleep(5000);
    }

    public async Task DisposeAsync()
    {
      await _eventStoreTestContainer.DeleteContainer();
    }
  }
}
