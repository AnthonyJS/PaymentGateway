using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGateway.Infrastructure.Security;

namespace PaymentGateway.API.Filters
{
  public class FullCardAccessAttribute : ActionFilterAttribute
  {
    private readonly IApiAuthorizationService _apiAuthorizationService;

    public FullCardAccessAttribute(IApiAuthorizationService apiAuthorizationService)
    {
      _apiAuthorizationService = apiAuthorizationService;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      // TODO: [] Handle if no authorization header
      // TODO: [] Handle if it is not a Guid
      // TODO: Logging
      var apiKey = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

      if (_apiAuthorizationService.ValidateFullCardAccess(new Guid(apiKey)))
      {
        base.OnActionExecuting(context);
        return;
      }
      
      context.Result = new UnauthorizedResult();
    }
  }
}
