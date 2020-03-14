using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;
using Xunit;
using System.Net;
using System.Threading.Tasks;
using PaymentGateway.API.Contracts.V1;
using API.Integration.Tests;

namespace PaymentGateway.API.Integration.Tests
{
  [Collection("sequential")]
  public class CreatePaymentTests : IntegrationTest
  {
    [Fact]
    public async Task ShouldInsertPaymentWithValidData()
    {
      var data = new CreatePaymentRequest()
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
      var response = await TestClient.PostAsync(ApiRoutes.Payments.Create, content);

      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      var responseData = JsonSerializer.Deserialize<CreatePaymentCommandResponse>(responseString);

      Assert.True(responseData.Id.GetType() == typeof(Guid));
    }

    [Fact]
    public async Task ShouldNotInsertPaymentWithInvalidData()
    {
      var data = new CreatePaymentRequest()
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
      var response = await TestClient.PostAsync(ApiRoutes.Payments.Create, content);

      Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
  }
}
