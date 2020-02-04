using System;

namespace PaymentGateway.Application.Requests
{
  public class CreateCustomerOrderRequest
  {
    public decimal Amount { get; set; }
  }
}