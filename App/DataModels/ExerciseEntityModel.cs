
namespace Treenirepository.DataModels
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

  public class ExerciseEntityModel : DbContext, IExerciseEntityModel
  {
    public static string ConnectionString { get; set; }
    
    /// <summary>
    /// Create function can be overwritten for testing purposes
    /// </summary>
    /// <returns><see cref="IExerciseEntityModel"/></returns>
    public static Func<IExerciseEntityModel> Create { get; set; } = CreateDatabaseModel;

    public ExerciseEntityModel(DbContextOptions<ExerciseEntityModel> options)
    : base(options)
    {
    }

    public virtual DbSet<Exercise> Exercises { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public DbSet<Section> Sections { get; set; }

    public async Task<int> SaveChangesAsync()
    {
      return await base.SaveChangesAsync(CancellationToken.None);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Exercise>(entity =>
        {
          entity.Property(e => e.Name).IsRequired();
        }
      );

    }
    private static IExerciseEntityModel CreateDatabaseModel()
    {
      if (ConnectionString != null)
      {
        var builder = new DbContextOptionsBuilder<ExerciseEntityModel>();
        builder.UseSqlServer(ConnectionString);

        return new ExerciseEntityModel(builder.Options);
      }
      else
      {
        return null;
      }
    }
  }
}