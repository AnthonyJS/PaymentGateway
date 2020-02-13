using System;
using System.Net.Http;
using PaymentGateway.Application.Responses;
using Xunit;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Enums;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;
using Newtonsoft.Json;

namespace PaymentGateway.API.Integration.Tests
{

  [Collection("sequential")]
  public class GetPaymentTests : IClassFixture<ApiClientFactory>, IClassFixture<PaymentHistoryRepository>
  {
    private readonly HttpClient _client;
    private readonly PaymentHistoryRepository _repository;

    public GetPaymentTests(ApiClientFactory factory, PaymentHistoryRepository repository)
    {
      _client = factory.CreateClient();
      _repository = repository;
    }

    [Fact]
    public async Task ShouldRetrievePayment()
    {
      var id = await insertPaymentDirectlyIntoDatabaseForTesting();

      var response = await _client.GetAsync($"/paymentgateway/{id}");

      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      Assert.NotNull(responseString);

      var responseData = JsonConvert.DeserializeObject<PaymentByIdResponse>(responseString);

      Assert.True(responseData.FirstName == "Jim");
      Assert.True(responseData.Amount == 4404.44M);
    }

    // TODO: Put this in a transaction so the data can be removed after the test finishes
    private async Task<Guid> insertPaymentDirectlyIntoDatabaseForTesting()
    {
      var payment = new Payment()
      {
        Id = Guid.NewGuid(),
        AcquiringBankId = Guid.NewGuid(),
        FirstName = "Jim",
        Surname = "Jimson",
        CardNumber = "1234-5678-8765-4321",
        ExpiryMonth = 10,
        ExpiryYear = 20,
        Currency = Currency.GBP,
        Amount = 4404.44M,
        CVV = 321
      };

      await _repository.InsertPayment(payment);

      return payment.Id;
    }
  }
}
