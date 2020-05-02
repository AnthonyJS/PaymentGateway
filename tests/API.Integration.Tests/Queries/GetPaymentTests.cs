using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentGateway.API.Contracts.V1;
using PaymentGateway.API.Contracts.V1.Responses;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.Enums;
using Xunit;

namespace PaymentGateway.API.Integration.Tests.Queries
{
  [Collection(CollectionName.PaymentTestCollection)]
  [Trait("Payments", "Get")]
  public class GetPaymentTests
  {
    private readonly PaymentTestFixture _fixture;

    public GetPaymentTests(PaymentTestFixture fixture)
    {
      _fixture = fixture;
    }

    [Fact]
    public async Task ShouldRetrievePayment()
    {
      var id = await insertPaymentDirectlyIntoDatabaseForTesting();

      var response = await _fixture.TestClient.GetAsync(ApiRoutes.Payments.Get.Replace("{Id}", id.ToString()));

      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      Assert.NotNull(responseString);

      var responseData = JsonConvert.DeserializeObject<GetPaymentByIdResponse>(responseString);

      Assert.True(responseData.FirstName == "Jim");
      Assert.True(responseData.Amount == 4404.44M);
    }

    [Fact]
    public async Task ShouldReturnErrorIfPaymentCannotBeFound()
    {
      var id = Guid.NewGuid();

      var response = await _fixture.TestClient.GetAsync(ApiRoutes.Payments.Get.Replace("{Id}", id.ToString()));

      Assert.True(response.StatusCode == HttpStatusCode.NotFound);
    }

    // TODO: Put this in a transaction so the data can be removed after the test finishes
    private async Task<Guid> insertPaymentDirectlyIntoDatabaseForTesting()
    {
      var cardDetails = new CardDetails(
        "Jim", 
        "Jimson", 
        "1234-5678-8765-4321", 
        10, 
        20, 
        321);
      var payment = new Payment(Guid.NewGuid(), cardDetails, Currency.GBP, 4404.44M);
      
      await _fixture.PaymentHistoryRepository.InsertPayment(payment);

      return payment.Id;
    }
  }
}
