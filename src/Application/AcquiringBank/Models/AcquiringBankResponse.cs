using System;
using PaymentGateway.Application.AcquiringBank.Enums;

namespace PaymentGateway.Application.AcquiringBank.Models
{
  public class AcquiringBankResponse
  {
    public Guid Id { get; set; }
    public AcquiringBankStatusCode StatusCode { get; set; }
  }
}