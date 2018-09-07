using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using treenirepository.Controllers;
using treenirepository.DataModels;
using Xunit;

namespace App.Tests
{
  public class SectionsControllerTests
  {

    /// <summary>
    /// Initialize the test data in the constructor
    /// </summary>
    public SectionsControllerTests()
    {
      // can be used to initialize test context
      var options = new DbContextOptionsBuilder<ExerciseEntityModel>()
          .UseInMemoryDatabase(databaseName: "testingdata");

      ExerciseEntityModel.optionsBuilder = options;

      // initalizeTestDb();
    }

    // [Fact]
    // public async Task TestContextInitialization()
    // {
    //         // use the context
    //         var sections = await context.Sections.ToListAsync();
    //         Assert.True(sections.Count == 1);
    // }

    [Fact]
    public async Task TestGet()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();

      await exerciseCtrl.CreateExerciseAsync(new treenirepository.Models.Exercise { Name = "test exercise", StartTime = DateTime.Now, Sections = new List<treenirepository.Models.Section>() });
      await sectionCtrl.CreateSectionAsync(new treenirepository.Models.Section { Name = "Section 1", Description = "testing object", Duration = 10, SetupDuration = 2, ExerciseId = 1 });


      var result = await exerciseCtrl.Get(1) as OkObjectResult;
      Assert.NotNull(result);

      dynamic payload = result.Value as treenirepository.Models.Exercise;
      Assert.True(payload.Id == 1);

      result = await sectionCtrl.Get(1) as OkObjectResult;
      Assert.NotNull(result);

      payload = result.Value as treenirepository.Models.Section;
      Assert.True(payload.Id == 1);
    }

    [Fact]
    public async Task TestUpdate()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();

      var result = await exerciseCtrl.Get(1) as OkObjectResult;
      Assert.NotNull(result);
      
      dynamic payload = result.Value as treenirepository.Models.Exercise;
      Assert.True(payload.Id == 1);
    }

    // private void initalizeTestDb()
    // {
    //     var options = new DbContextOptionsBuilder<ExerciseEntityModel>()
    //         .UseInMemoryDatabase(databaseName: "testingdata")
    //         .Options;

    //         context =  new ExerciseEntityModel(options);

    //         var ex = new Exercise{Name = "test exercise",  StartTime = DateTime.Now, Sections = new List<Section>() };
    //         ex.Sections.Add(new Section{Name = "Section 1", Description = "testing object", Duration = 10, SetupDuration = 2, Exercise = ex});
    //         context.Exercises.Add(
    //             ex
    //             );
    //         context.SaveChangesAsync();
    // }
  }
}
