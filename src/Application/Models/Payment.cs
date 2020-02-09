using System;
using PaymentGateway.Application.Enums;

namespace PaymentGateway.Application.Models
{
  public class Payment
  {
    public Guid Id { get; set; }
    public Guid AcquiringBankId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
  }
}