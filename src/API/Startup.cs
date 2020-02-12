using System;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PipelineBehaviours;
using PaymentGateway.Infrastructure.ExternalAPIs.AcquiringBank;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;

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
      services.AddControllers();
      services.AddScoped<IAcquiringBankService, AcquiringBankService>();
      services.AddScoped<IAcquiringBankHttpClient, FakeAcquiringBankHttpClient>();
      services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();

      var assembly = AppDomain.CurrentDomain.Load("PaymentGateway.Application");
      services.AddMediatR(assembly);
      services.AddAutoMapper(typeof(Startup));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      services.AddValidatorsFromAssembly(assembly);
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

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
