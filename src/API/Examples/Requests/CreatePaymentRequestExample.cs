using PaymentGateway.Application.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.API.Examples.Requests
{
  public class CreatePaymentRequestExample : IExamplesProvider<CreatePaymentRequest>
  {
    public CreatePaymentRequest GetExamples()
    {
      return new CreatePaymentRequest
      {
        FirstName = "Tim",
        Surname = "Tomson",
        CardNumber = "1234-5678-8765-4321",
        ExpiryMonth = 10,
        ExpiryYear = 20,
        Currency = "GBP",
        Amount = 4404.44M,
        CVV = 321
      };
    }
  }
}
