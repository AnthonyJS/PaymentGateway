using System;

namespace PaymentGateway.Infrastructure.Persistence.PaymentHistory
{
  public class PaymentDTO
  {
    public Guid Id { get; set; }
    public Guid AcquiringBankId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public short CVV { get; set; }
    public int PaymentStatusId { get; set; }
  }
}
