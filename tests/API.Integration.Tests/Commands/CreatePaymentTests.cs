using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentGateway.API.Application.Commands;
using PaymentGateway.API.Contracts.V1;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.API.Contracts.V1.Responses;
using Xunit;

namespace PaymentGateway.API.Integration.Tests.Commands
{
  [Collection(CollectionName.PaymentTestCollection)]
  [Trait("Payments", "Create")]
  public class CreatePaymentTests
  {
    private readonly PaymentTestFixture _fixture;

    public CreatePaymentTests(PaymentTestFixture fixture)
    {
      _fixture = fixture;
    }
    
    [Fact]
    public async Task ShouldInsertPaymentWithValidData()
    {
      var data = new CreatePaymentCommand()
      {
        FirstName = "Jim",
        Surname = "Jimson",
        CardNumber = "1234-5678-8765-4321",
        ExpiryMonth = 10,
        ExpiryYear = 20,
        Currency = "GBP",
        Amount = 4404.44M,
        CVV = 321
      };
      var payload = JsonSerializer.Serialize(data);

      var content = new StringContent(payload, Encoding.UTF8, "application/json");
      var response = await _fixture.TestClient.PostAsync(ApiRoutes.Payments.Create, content);

      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      var responseData = JsonSerializer.Deserialize<CreatePaymentSuccessResponse>(responseString);

      Assert.True(responseData.Id.GetType() == typeof(Guid));
    }

    [Fact]
    public async Task ShouldNotInsertPaymentWithInvalidMonth()
    {
      var data = new CreatePaymentCommand()
      {
        FirstName = "Jim",
        Surname = "Jimson",
        CardNumber = "1234-5678-8765-4321",
        ExpiryMonth = 15, // Invalid month
        ExpiryYear = 20,
        Currency = "GBP",
        Amount = 4404.44M,
        CVV = 321
      };
      var payload = JsonSerializer.Serialize(data);

      var content = new StringContent(payload, Encoding.UTF8, "application/json");
      var response = await _fixture.TestClient.PostAsync(ApiRoutes.Payments.Create, content);

      Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ShouldNotInsertPaymentWithInvalidAmount()
    {
      var data = new CreatePaymentCommand()
      {
        FirstName = "Jim",
        Surname = "Jimson",
        CardNumber = "1234-5678-8765-4321",
        ExpiryMonth = 10, 
        ExpiryYear = 20,
        Currency = "GBP",
        Amount = 44404.44M, // Amount too high
        CVV = 321
      };
      var payload = JsonSerializer.Serialize(data);

      var content = new StringContent(payload, Encoding.UTF8, "application/json");
      var response = await _fixture.TestClient.PostAsync(ApiRoutes.Payments.Create, content);

      Assert.True(response.StatusCode == HttpStatusCode.UnprocessableEntity);
    }
  }
}
