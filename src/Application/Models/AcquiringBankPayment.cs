using System;

namespace PaymentGateway.Application.Models
{
  public class AcquiringBankPayment
  {
    public Guid Id { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
  }
}
