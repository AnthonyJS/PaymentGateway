using System.Collections.Generic;

namespace PaymentGateway.Infrastructure.Security
{
  public class AuthenticationOptions
  {
    public IEnumerable<ApiKey> ApiKeys { get; set; }
  }
}
