// <copyright file="Program.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.Logging;

  /// <summary>
  /// StartUp class.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Standard startup method.
    /// </summary>
    /// <param name="args">An array of string arguments that can be supplied when starting program.</param>
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Dotnet Core webhost setup.
    /// </summary>
    /// <param name="args">An array of string arguments that can be supplied when starting program.</param>
    /// <returns>Instance of <see cref="IWebHostBuilder"/>.</returns>
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
