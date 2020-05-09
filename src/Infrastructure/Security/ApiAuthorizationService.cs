using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PaymentGateway.Infrastructure.Security
{
  public interface IApiAuthorizationService
  {
    bool ValidateFullCardAccess(string apiKey);
  }

  public class ApiAuthorizationService : IApiAuthorizationService
  {
    private readonly ILogger<ApiAuthorizationService> _logger;
    private readonly AuthenticationOptions _authenticationOptions;

    public ApiAuthorizationService(IOptions<AuthenticationOptions> options, ILogger<ApiAuthorizationService> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _authenticationOptions = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
    }

    public bool ValidateFullCardAccess(string apiKey)
    {
      if (string.IsNullOrEmpty(apiKey))
      {
        _logger.LogInformation($"Header Authorization not provided");
        return false;
      }
      
      if (!Guid.TryParse(apiKey, out _))
      {
        _logger.LogInformation($"Header Authorization not a Guid {apiKey}");
        return false;
      }
      
      var apiValues = _authenticationOptions.ApiKeys.FirstOrDefault(a => a.Key.Equals(apiKey));

      if (apiValues == null)
      {
        _logger.LogInformation($"Header Authorization Guid not known {apiKey}");
        return false;
      }
      
      _logger.LogInformation($"Header Authorization {apiKey} has card access: {apiValues.HasFullCardAccess}");
      return apiValues.HasFullCardAccess;
    }
  }
}
