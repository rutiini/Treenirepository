
namespace Treenirepository
{

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using Treenirepository.DataModels;

  /// <summary>
  /// Startup class for setting up services and configurations.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Standard startup method.
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Standard startup method. Gets the connectionstring from settings and sets it in the controller property.
    /// </summary>
    /// <value></value>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      var connection = Configuration["Production:ConnectionString"];

      //services.AddDbContext<ExerciseEntityModel>(options => options.UseSqlServer(connection));
      ExerciseEntityModel.ConnectionString = connection;

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env">Environment (Debug/Release)</param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }

  }
}
