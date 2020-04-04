using System;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.API.Core;
using PaymentGateway.API.Options;

namespace PaymentGateway.API
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
      services.AddSwagger();

      // Won't be needed once App.Metrics 4.0.0 is out of preview
      services.Configure<KestrelServerOptions>(options =>
      {
        options.AllowSynchronousIO = true;
      });
      services.AddMetrics();
      
      services.AddControllers();
      services.AddDependencyInjectionWireup();

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
      app.UseStaticFiles();

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
  }
}
