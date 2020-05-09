using System;

namespace PaymentGateway.Infrastructure.Security
{
  public class ApiKey
  {
    public string Key { get; set; }
    public bool HasFullCardAccess { get; set; }
  }
}
