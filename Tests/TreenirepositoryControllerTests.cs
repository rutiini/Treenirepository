using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Treenirepository.Controllers;
using Treenirepository.DataModels;
using Xunit;

namespace App.Tests
{
  public class TreenirepositoryContrrollerTests
  {

    /// <summary>
    /// Set up the inmemory database for testing.
    /// </summary>
    public TreenirepositoryContrrollerTests()
    {
      // can be used to initialize test context
      ExerciseEntityModel.Create = () => GetTestDb();
    }
    [Fact]
    public async Task CreateExerciseAsync_CreateSectionAsync_Success()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();

      // create in sequence
      // create exercise without sections
      string exerciseName = "test exercise";
      int exerciseId = ((await exerciseCtrl.CreateExerciseAsync(
        new Treenirepository.Models.Exercise
        {
          Name = exerciseName,
          StartTime = DateTime.Now,
          Sections = new List<Treenirepository.Models.Section>()
        }) as OkObjectResult)
          .Value as Treenirepository.Models.Exercise)
          .Id;

      // create section for existing exercise
      string sectionName = "Section 1";
      int sectionId = ((await sectionCtrl.CreateSectionAsync(
        new Treenirepository.Models.Section
        {
          Name = sectionName,
          Description = "testing object",
          Duration = 10,
          SetupDuration = 2,
          ExerciseId = exerciseId
        }) as OkObjectResult)
        .Value as Treenirepository.Models.Section)
        .Id;

      var result = await exerciseCtrl.Get(exerciseId) as OkObjectResult;
      Assert.NotNull(result);

      dynamic payload = result.Value as Treenirepository.Models.Exercise;
      Assert.True(payload.Name == exerciseName);

      result = await sectionCtrl.Get(sectionId) as OkObjectResult;
      Assert.NotNull(result);

      payload = result.Value as Treenirepository.Models.Section;
      Assert.True(payload.Name == sectionName);
    }

    [Fact]
    public async Task CreateExerciseAsync_CreateSectionAsync_Chained_Success()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();
      // we can also use task chaining
      int createdExerciseId = 0;
      string createdExerciseName = "test exercise2";
      await exerciseCtrl.CreateExerciseAsync(
        new Treenirepository.Models.Exercise
        {
          Name = createdExerciseName,
          StartTime = DateTime.Now,
          Sections = new List<Treenirepository.Models.Section>()
        })
        .ContinueWith(async (exerciseresult) =>
        {
          if ((exerciseresult.Result as OkObjectResult) != null)
          {
            createdExerciseId = ((exerciseresult.Result as OkObjectResult).Value as Treenirepository.Models.Exercise).Id;
            await sectionCtrl.CreateSectionAsync(
              new Treenirepository.Models.Section
              {
                Name = "Section 2.1",
                Description = "testing object",
                Duration = 15,
                SetupDuration = 3,
                ExerciseId = createdExerciseId
              });
          }

        });

      Assert.True(createdExerciseId != 0);
      var result = await exerciseCtrl.Get(createdExerciseId) as OkObjectResult;
      Assert.NotNull(result);

      var payload = result.Value as Treenirepository.Models.Exercise;
      Assert.True(payload.Name == createdExerciseName);
    }

    [Fact]
    public async Task CreateExerciseAsync_WithSections_Success()
    {
      var exerciseCtrl = new ExercisesController();

      // create in sequence
      string sectionName = "Section 1";
        var section1 = new Treenirepository.Models.Section
        {
          Name = sectionName,
          Description = "testing object",
          Duration = 10,
          SetupDuration = 2
        };
        string section2Name = "Section 2";
        var section2 = new Treenirepository.Models.Section
        {
          Name = section2Name,
          Description = "testing object",
          Duration = 15,
          SetupDuration = 0
        };
      // create exercise with sections
      string exerciseName = "test exercise";
      int exerciseId = ((await exerciseCtrl.CreateExerciseAsync(
        new Treenirepository.Models.Exercise
        {
          Name = exerciseName,
          StartTime = DateTime.Now,
          Sections = new List<Treenirepository.Models.Section>(){section1, section2}
        }) as OkObjectResult)
          .Value as Treenirepository.Models.Exercise)
          .Id;

      var result = await exerciseCtrl.Get(exerciseId) as OkObjectResult;
      Assert.NotNull(result);

      var payload = result.Value as Treenirepository.Models.Exercise;
      
      // check the returned objects references and integrity
      Assert.True(payload.Name == exerciseName);
      Assert.True(payload.Sections.Count == 2);
      Assert.True(payload.Sections.First().ExerciseId == payload.Id);
    }

    [Fact]
    public async Task UpdateExerciseAsync_Success()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();

      var result = await exerciseCtrl.Get(1) as OkObjectResult;
      Assert.NotNull(result);

      var payload = result.Value as Treenirepository.Models.Exercise;
      Assert.True(payload.Id == 1);
      string updatedExerciseName = "modified exercise";
      payload.Name = updatedExerciseName;
      payload.Sections.Add(
        new Treenirepository.Models.Section
        {
          Name = "added section", 
          Description = "lets see if this causes problems kjeh kjeh", 
          Duration = 12, 
          SetupDuration = 1, 
          Color = 3 
        });

      result = await exerciseCtrl.UpdateExerciseAsync(payload) as OkObjectResult;
      payload =  result.Value as Treenirepository.Models.Exercise;
      Assert.True(payload.Name == updatedExerciseName);
    }

    private bool testDbPopulated = false;

    /// <summary>
    /// Initialize the underlying database for testing.
    /// TODO: Consider making the process async.
    /// </summary>
    /// <returns></returns>
    private IExerciseEntityModel GetTestDb()
    {
      // Modify the underlying context to create an inmemory database with alternative create function.
      var builder = new DbContextOptionsBuilder<ExerciseEntityModel>()
            .UseInMemoryDatabase(databaseName: "testingdata");

      var context = new ExerciseEntityModel(builder.Options);

      // Populate the test context with sample data ONCE. The implementation should be safe for multiple asynchronous test tasks running simultaneously.
      if (!testDbPopulated)
      {
        testDbPopulated = true;
        var ex = new Exercise { Name = "test exercise", StartTime = DateTime.Now, Sections = new List<Section>() };
        ex.Sections.Add(new Section { Name = "Section 1", Description = "testing object", Duration = 10, SetupDuration = 2, Exercise = ex });
        context.Exercises.Add(
            ex
            );

        // is this OK?
        Task.WaitAll(context.SaveChangesAsync());
      }

      return context;
    }
  }
}
