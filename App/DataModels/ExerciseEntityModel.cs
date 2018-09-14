// <copyright file="ExerciseEntityModel.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.DataModels
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

  /// <inheritdoc/>
  public class ExerciseEntityModel : DbContext, IExerciseEntityModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExerciseEntityModel"/> class.
    /// </summary>
    /// <param name="options">Context options <see cref="DbContextOptions"/>.</param>
    public ExerciseEntityModel(DbContextOptions<ExerciseEntityModel> options)
    : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the database connection string property used to configure the context.
    /// Internal functionality will handle rest of the necessary configurations.
    /// </summary>
    /// <value>A valid connectionstring.</value>
    public static string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets a function which can be overwritten for testing purposes.
    /// </summary>
    /// <value>Function for that returns an instance of the entity model such as <see cref="CreateDatabaseModel()"/>.</value>
    public static Func<IExerciseEntityModel> Create { get; set; } = CreateDatabaseModel;

    /// <inheritdoc/>
    public virtual DbSet<Exercise> Exercises { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<User> Users { get; set; }

    /// <inheritdoc/>
    public DbSet<Section> Sections { get; set; }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
      return await base.SaveChangesAsync(CancellationToken.None);
    }

    /// <summary>
    /// Experimental validator method.
    /// </summary>
    /// <param name="modelBuilder">Model builder for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Exercise>(entity =>
        {
          entity.Property(e => e.Name).IsRequired();
        });
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