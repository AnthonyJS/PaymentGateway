using System;
using FluentValidation;
using PaymentGateway.API.Contracts.V1.Requests;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.API.Validation
{
  public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
  {
    public CreatePaymentRequestValidator()
    {
      RuleFor(x => x.FirstName).NotEmpty().Length(1, 100).WithMessage("Please enter your First name");
      RuleFor(x => x.Surname).NotEmpty().Length(1, 100).WithMessage("Please enter your Surname");
      RuleFor(x => x.CardNumber).NotEmpty().Length(1, 20).WithMessage("Please enter a 16 digit card number");
      RuleFor(x => x.ExpiryMonth).NotEmpty().Must(month => month > 0 && month <= 12).WithMessage("Please enter the month as a number, e.g. 1 for January");
      RuleFor(x => x.ExpiryYear).NotEmpty().Must(year => year >= 0 && year <= 99).WithMessage("Please enter the year as 2 digits, e.g. 22 for 2022");

      RuleFor(x => x.Currency).NotEmpty();
      RuleFor(x => x.Currency).Must(c => Enum.TryParse(c, out Currency _)).WithMessage("Please enter GBP, USD, EUR, AUD or JPY");

      RuleFor(x => x.Amount).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Please enter an amount that is greater than 0 units");

      RuleFor(x => x.CVV).NotEmpty().Must(cvv => cvv >= 100 && cvv <= 999).WithMessage("Please enter a 3 digit CVV number");
    }
  }
}
