using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.API.Core
{
  public static class ConfigureSwaggerExtensions
  {
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
      services.AddSwaggerGen(x =>
      {
        x.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway", Version = "v1" });

        x.ExampleFilters();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        x.IncludeXmlComments(xmlPath);
      });

      services.AddSwaggerExamplesFromAssemblyOf<Startup>();

      return services;
    }
  }
}
