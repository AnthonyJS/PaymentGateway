using System;

namespace PaymentGateway.Infrastructure.Security
{
  public class ApiKey
  {
    public Guid Key { get; set; }
    public bool HasFullCardAccess { get; set; }
  }
}
