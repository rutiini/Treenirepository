
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

  public class SectionCotrollerTests
  {

    public SectionCotrollerTests()
    {
      ExerciseEntityModel.Create = () => GetTestDb();
    }

    [Fact]
    public async Task GetSectionAsync_Success()
    {
      var sectionCtrl = new SectionsController();
      
      // section with id 1 is always available unless deleted.
      var result = ((await sectionCtrl.Get(1) as OkObjectResult)
      .Value as Treenirepository.Models.Section);
      
      Assert.NotNull(result);
      Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreateSectionAsync_Success()
    {
      var sectionCtrl = new SectionsController();
      var exerciseCtrl = new ExercisesController();

      int exerciseId = ((await exerciseCtrl.Get() as OkObjectResult)
      .Value as IEnumerable<Treenirepository.Models.Exercise>)
      .First()
      .Id;
      
      Assert.True(exerciseId > 0);

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

      var result = await sectionCtrl.Get(sectionId) as OkObjectResult;
      Assert.NotNull(result);

      var payload = result.Value as Treenirepository.Models.Section;
      Assert.True(payload.Name == sectionName);
    }

    [Fact]
    public async Task UpdateSectionAsync_Success()
    {
      var sectionCtrl = new SectionsController();

      var section = ((await sectionCtrl.Get(1) as OkObjectResult)
      .Value as Treenirepository.Models.Section);
      
      string modifiedName = $"{section.Name} modified";
      section.Name = modifiedName;
      
      section = ((await sectionCtrl.UpdateSectionAsync(section) as OkObjectResult)
      .Value as Treenirepository.Models.Section);

      Assert.NotNull(section);
      Assert.True(section.Name == modifiedName);
    }

    [Fact]
    public async Task UpdateSectionAsync_Fail()
    {
      var sectionCtrl = new SectionsController();

      var section = ((await sectionCtrl.Get(1) as OkObjectResult)
      .Value as Treenirepository.Models.Section);
      
      string modifiedName = $"{section.Name} modified";
      var faultySection = 
      new Treenirepository.Models.Section
      { 
        Name = modifiedName, 
        SetupDuration = 1, 
        Description = "this section is missing duration info and ExerciseId", 
        Color = 3
      };
      
      var response = await sectionCtrl.UpdateSectionAsync(faultySection) as BadRequestObjectResult;

      Assert.NotNull(response);
    }
    private bool testDbPopulated = false;
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
