using PaymentGateway.API.Application.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.API.Examples.Requests
{
  public class CreatePaymentRequestExample : IExamplesProvider<CreatePaymentCommand>
  {
    public CreatePaymentCommand GetExamples()
    {
      return new CreatePaymentCommand
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
