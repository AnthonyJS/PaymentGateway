using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace PaymentGateway.Infrastructure.Security
{
  public interface IApiAuthorizationService
  {
    bool ValidateFullCardAccess(Guid apiKey);
  }

  public class ApiAuthorizationService : IApiAuthorizationService
  {
    private readonly AuthenticationOptions _authenticationOptions;

    public ApiAuthorizationService(IOptions<AuthenticationOptions> options)
    {
      _authenticationOptions = options.Value;
    }

    public bool ValidateFullCardAccess(Guid apiKey)
    {
      // TODO: [] ApiKey.Key might need to be string rather than Guid
      // TODO: [] Validation on ApiKey strings from config?
      var apiValues = _authenticationOptions.ApiKeys.FirstOrDefault(a => a.Key.Equals(apiKey));

      if (apiValues == null)
        return false;

      return apiValues.HasFullCardAccess;
    }
  }
}
