
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
      // test can fail if we try to update a non-existing section. other stuff should be caught by the validation.
      var faultySection =
      new Treenirepository.Models.Section
      {
        Id = 657768,
        Name = modifiedName,
        SetupDuration = 1,
        Duration = 10,
        Description = "this section is missing duration info and ExerciseId",
        Color = 3
      };

      var response = await sectionCtrl.UpdateSectionAsync(faultySection) as BadRequestObjectResult;

      Assert.NotNull(response);
    }

    /// <summary>
    /// A bit of load testing by firing of a bunch of get requests.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    public async Task Get_LoadTest(int amount)
    {
      var sectionCtrl = new SectionsController();
      List<Task<IActionResult>> taskList = new List<Task<IActionResult>>();
      var list = Enumerable.Range(1, amount).ToList();

      foreach (var item in list)
      {
        System.Console.WriteLine($"starting request {item}");
        taskList.Add(sectionCtrl.Get(1));
      }

      var results = await Task.WhenAll(taskList.ToArray());
      // check if any of the calls resulted in other than OkObjectResult
      Assert.True(!taskList.Any(t => (t.Result as OkObjectResult) == null));
      Assert.True(taskList.Count == amount);
    }

    /// <summary>
    /// A bit of load testing by firing of a bunch of create requests.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    public async Task Create_LoadTest(int amount)
    {
      var sectionCtrl = new SectionsController();

      List<Task<IActionResult>> taskList = new List<Task<IActionResult>>();
      var list = Enumerable.Range(1, amount).ToList();

      foreach (var item in list)
      {
        var newSection =
      new Treenirepository.Models.Section
      {
        Id = 1, // always in the test db..
        Name = $"new section number {item}",
        SetupDuration = 1,
        Duration = 10,
        Description = "this section is missing duration info and ExerciseId",
        Color = 3,
        ExerciseId = 1
      };
        System.Console.WriteLine($"starting request {item}");
        taskList.Add(sectionCtrl.CreateSectionAsync(newSection));
      }

      var results = await Task.WhenAll(taskList.ToArray());
      // check if any of the calls resulted in other than OkObjectResult
      Assert.True(!taskList.Any(t => (t.Result as OkObjectResult) == null));
      Assert.True(taskList.Count == amount);
    }

    /// <summary>
    /// A bit of load testing by firing of a bunch of create requests.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    public async Task Update_LoadTest(int amount)
    {
      var sectionCtrl = new SectionsController();

      List<Task<IActionResult>> taskList = new List<Task<IActionResult>>();
      var list = Enumerable.Range(1, amount).ToList();

      foreach (var item in list)
      {
        var newSection =
      new Treenirepository.Models.Section
      {
        Id = 1, // always in the test db..
        Name = $"new section number {item}",
        SetupDuration = 1,
        Duration = 10,
        Description = "this section is missing duration info and ExerciseId",
        Color = 3
      };
        System.Console.WriteLine($"starting request {item}");
        taskList.Add(sectionCtrl.UpdateSectionAsync(newSection));
      }

      var results = await Task.WhenAll(taskList.ToArray());

      Assert.IsType(typeof(OkObjectResult),results.Last());
      Assert.IsType(typeof(OkObjectResult),results.First());
      // check if any of the calls resulted in other than OkObjectResult
      Assert.True(!taskList.Any(t => (t.Result as OkObjectResult) == null));
      Assert.True(taskList.Count == amount);

      var formattedResult = results.Last() as OkObjectResult;
      Assert.NotNull(formattedResult);
      var payload = formattedResult.Value as Treenirepository.Models.Section;
      Assert.True(payload.Id == 1);
    }

    // /// <summary>
    // /// Validation checking needs to be done throug actual calls to the api
    // /// or calling the tryvalidate method. For some reason the validation messages are not being passed.
    // /// </summary>
    // /// <returns>Task</returns>
    // [Fact]
    // public async Task Validation_Checking_With_Http_Client()
    // {
    //   Assert.True(1 == 1);

    //   // we need the backend running for these kind of tests!
    //   /*
    //   var client = new HttpClient();
    //   var faultySection = new Treenirepository.Models.Section
    //   {
    //     Id = 657768,
    //     Name = "faulty duration",
    //     SetupDuration = 1,
    //     Duration = 0,
    //     Description = "this section is missing duration info and ExerciseId",
    //     Color = 3
    //   };
    //   var jsonInString = JsonConvert.SerializeObject(faultySection);
    //   var response = await client.PostAsync("http://localhost:5000/sections/create", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
    //   */
    //   /*
    //   var config = new HttpConfiguration();
    //   config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
    //   // additional config ...
    //   var server = new HttpServer(config);
    //   var client = new HttpClient(server);
    //   var r = client.GetAsync("http://can.be.anything/resource")
    //   */
    // }

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
