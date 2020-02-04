using System;

namespace PaymentGateway.Application.Requests
{
  public class CreateCustomerOrderRequest
  {
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
  }
}