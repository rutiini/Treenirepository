// <copyright file="IExerciseEntityModel.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.DataModels
{
  using System;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Exercise entity database context model used to manipulate database entities.
  /// </summary>
  public interface IExerciseEntityModel : IDisposable
  {
    /// <summary>
    /// Gets Exercises objects in the database.
    /// </summary>
    /// <value>DbSet of Exercises.</value>
    DbSet<Exercise> Exercises { get; }

    /// <summary>
    /// Gets Users objects in the database.
    /// </summary>
    /// <value>DbSet of Users.</value>
    DbSet<User> Users { get; }

    /// <summary>
    /// Gets Sections objects in the database.
    /// </summary>
    /// <value>DbSet of Sections.</value>
    DbSet<Section> Sections { get; }

    /// <summary>
    /// Perform an asynchronous save of the context to the database.
    /// </summary>
    /// <returns>Int that indicates task result.</returns>
    Task<int> SaveChangesAsync();
  }
}