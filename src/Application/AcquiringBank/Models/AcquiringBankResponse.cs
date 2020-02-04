using System;
using PaymentGateway.Application.AcquiringBank.Enums;

namespace PaymentGateway.Application.AcquiringBank.Models
{
  public class AcquiringBankResponse
  {
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public AcquiringBankStatusCode StatusCode { get; set; }
  }
}