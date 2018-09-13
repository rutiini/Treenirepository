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
    /// Access to Exercises objects in the database.
    /// </summary>
    /// <value></value>
    DbSet<Exercise> Exercises { get; }

    /// <summary>
    /// Access to Users objects in the database.
    /// </summary>
    /// <value></value>
    DbSet<User> Users { get; }

    /// <summary>
    /// Access to Sections objects in the database.
    /// </summary>
    /// <value></value>
    DbSet<Section> Sections { get; }

    /// <summary>
    /// Perform an asynchronous save of the context to the database.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
  }
}