using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.SeedWork;

namespace PaymentGateway.Domain.AggregatesModel.PaymentAggregate
{
  public class CardDetails : ValueObject
  {
    public CardDetails(string firstName, string surname, string cardNumber, int expiryMonth, int expiryYear, short cvv)
    {
      FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new PaymentDomainException(nameof(firstName));
      Surname = !string.IsNullOrWhiteSpace(surname) ? surname : throw new PaymentDomainException(nameof(surname));
      CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new PaymentDomainException(nameof(cardNumber));
      ExpiryMonth = expiryMonth != 0 ? expiryMonth : throw new PaymentDomainException(nameof(expiryMonth));
      ExpiryYear = expiryYear != 0 ? expiryYear : throw new PaymentDomainException(nameof(expiryYear));
      CVV = cvv != 0 ? cvv : throw new PaymentDomainException(nameof(cvv));
    }
    
    public string FirstName { get; }
    public string Surname { get; }
    public string CardNumber { get; }
    public int ExpiryMonth { get; }
    public int ExpiryYear { get; }
    public short CVV { get; }
  }
}
