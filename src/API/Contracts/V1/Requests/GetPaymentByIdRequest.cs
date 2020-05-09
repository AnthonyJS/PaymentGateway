using System;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.API.Contracts.V1.Requests
{
  public class GetPaymentByIdRequest 
  {
    [FromRoute]
    public Guid Id { get; set; }
  }
}


