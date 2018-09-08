

namespace Treenirepository.DataModels
{
  using System;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;

  public interface IExerciseEntityModel : IDisposable
  {
    DbSet<Exercise> Exercises { get; }
    DbSet<User> Users { get; }
    DbSet<Section> Sections { get; }

    Task<int> SaveChangesAsync();
  }
}