using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.API.Options;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      configureSwagger(services);
      services.AddControllers();
      services.AddScoped<IAcquiringBankService, AcquiringBankService>();
      services.AddScoped<IAcquiringBankHttpClient, FakeAcquiringBankHttpClient>();
      services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();

      var applicationAssembly = AppDomain.CurrentDomain.Load("PaymentGateway.Application");
      services.AddMediatR(applicationAssembly);
      services.AddAutoMapper(typeof(Startup));
      services.AddControllers()
        .AddFluentValidation(opt =>
        {
          opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      var swaggerOptions = new SwaggerOptions();
      Configuration.Bind(nameof(SwaggerOptions), swaggerOptions);

      app.UseSwagger(option =>
      {
        option.RouteTemplate = swaggerOptions.JsonRoute;
      });

      app.UseSwaggerUI(option =>
      {
        option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    private void configureSwagger(IServiceCollection services)
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
    }
  }
}
