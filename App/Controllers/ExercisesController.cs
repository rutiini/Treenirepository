
namespace treenirepository.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using treenirepository.DataModels;
  using treenirepository.Models;

  [Route("api/[controller]")]
  public class ExercisesController : Controller
  {
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Exercise>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
      return Ok(await GetExercisesFromDbAsync());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Exercise>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        return Ok(await GetExerciseFromDbAsync(id));
      }
      catch
      {
        return NotFound($"no section exists with id {id}");
      }
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(IEnumerable<Models.Exercise>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateExerciseAsync([FromBody] Models.Exercise newExercise)
    {
      return Ok(await CreateExerciseToDbAsync(newExercise));
    }

    [HttpGet]
    [Route("{exerciseId:int}/sections")]
    [ProducesResponseType(typeof(IEnumerable<Models.Section>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetExerciseSections(int exerciseId)
    {
      return Ok(await GetExerciseSectionsFromDbAsync(exerciseId));
    }

    private async Task<IEnumerable<Models.Section>> GetExerciseSectionsFromDbAsync(int exerciseId)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        return await context.Sections
        .AsNoTracking()
        .Where(s => s.ExerciseId == exerciseId)
        .Select(s =>
        new Models.Section(s))
        .ToListAsync();
      }
    }

    private async Task<IEnumerable<Models.Exercise>> GetExercisesFromDbAsync()
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        // db operation as async
        var exercises = await context.Exercises
          .Include("Sections")
          .AsNoTracking()
          .ToListAsync();

        // return formatting
        return exercises.Select(e =>
          new Models.Exercise(e));
      }
    }
    private async Task<Models.Exercise> GetExerciseFromDbAsync(int id)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        // db operation as async
        var exercise = await context.Exercises
          .Include("Sections")
          .AsNoTracking()
          .SingleAsync(e => e.Id == id);

        return new Models.Exercise(exercise);
      }
    }

    private async Task<IEnumerable<Models.Exercise>> CreateExerciseToDbAsync(Models.Exercise newExercise)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        await context.Exercises.AddAsync(new DataModels.Exercise(newExercise));

        await context.SaveChangesAsync();

      }
      return newExercise as IEnumerable<Models.Exercise>;
    }
  }

}