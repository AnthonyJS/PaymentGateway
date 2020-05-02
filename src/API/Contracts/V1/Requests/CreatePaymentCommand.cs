using CSharpFunctionalExtensions;
using MediatR;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;

namespace PaymentGateway.API.Application.Commands
{
  public class CreatePaymentCommand : IRequest<Result<Payment>>
  {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public byte ExpiryMonth { get; set; }
    public byte ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
  }


}
