using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using PaymentGateway.API;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;

namespace PaymentGateway.API.Integration.Tests
{
  public class PaymentTestFixture
  {
    public IPaymentHistoryRepository PaymentHistoryRepository { get; }
    public HttpClient TestClient { get; }

    public PaymentTestFixture()
    {
      PaymentHistoryRepository = new PaymentHistoryRepository();
      var appFactory = new WebApplicationFactory<Startup>();
      TestClient = appFactory.CreateClient();         
    }
  }
}
