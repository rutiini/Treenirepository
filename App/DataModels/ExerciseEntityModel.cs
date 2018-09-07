
namespace treenirepository.DataModels
{
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

  public class ExerciseEntityModel : DbContext, IExerciseEntityModel
  {
    public static string ConnectionString { get; set; }

    /// <summary>
    /// used for creating test contexts
    /// </summary>
    /// <value></value>
    public static DbContextOptionsBuilder<ExerciseEntityModel> optionsBuilder {get; set;}

    //public ExerciseEntityModel() { }
    public static IExerciseEntityModel Create() => CreateDatabaseModel();

    public ExerciseEntityModel(DbContextOptions<ExerciseEntityModel> options)
    : base(options)
    {
      // var extension = options.FindExtension<SqlServerOptionsExtension>();

      // ConnectionString = extension.ConnectionString;
    }

    public virtual DbSet<Exercise> Exercises { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public DbSet<Section> Sections { get; set; }

    public async Task<int> SaveChangesAsync()
    {
      return await base.SaveChangesAsync(CancellationToken.None);
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //   if (!optionsBuilder.IsConfigured)
    //   {
    //     var opts = optionsBuilder.Options;
    //     var extension = opts.FindExtension<SqlServerOptionsExtension>();

    //     ConnectionString = extension.ConnectionString;
    //   }

    // }

    /// Validation? -> can be done with annotations well enough
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
      else if(optionsBuilder != null)
      {
        return new ExerciseEntityModel(optionsBuilder.Options);
      }
      else
      {
        return null;
      }
    }
  }
}