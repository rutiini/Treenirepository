// <copyright file="Startup.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
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
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">App configurations.</param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Gets the connectionstring from settings and sets it in the controller property.
    /// </summary>
    /// <value>Configuration property: getter.</value>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">Services that should be configured.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      var connection = Configuration["Production:ConnectionString"];

      // services.AddDbContext<ExerciseEntityModel>(options => options.UseSqlServer(connection));
      ExerciseEntityModel.ConnectionString = connection;

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">Application builder for configuring application request pipeline.</param>
    /// <param name="env">Environment (Debug/Release).</param>
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
