
namespace Treenirepository.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Treenirepository.DataModels;
  using Treenirepository.Models;

  [Route("api/[controller]")]
  [ApiController]
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
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
      var result = await GetExerciseFromDbAsync(id);
      try
      {
        if (result != null)
        {
          return Ok(result);
        }
        else
        {
          return NotFound($"no section exists with id {id}");
        }
      }
      catch (System.Exception)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError);
      }
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(Models.Exercise), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateExerciseAsync([FromBody] Models.Exercise newExercise)
    {
      try
      {
        var result = await CreateExerciseToDbAsync(newExercise);
        return Ok(result);
      }
      catch (System.Exception e)
      {
        System.Console.WriteLine(e.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError);
      }
    }

    [HttpPost]
    [Route("update")]
    [ProducesResponseType(typeof(Models.Exercise), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> UpdateExerciseAsync([FromBody] Models.Exercise updatedExercise)
    {
      try
      {
        var updateResult = await UpdateExerciseToDbAsync(updatedExercise);
        if (updateResult != null)
        {
          return Ok(updateResult);
        }
        else
        {
          return NotFound($"could not find exercise with id {updatedExercise.Id} to update.");
        }
      }
      catch (Exception)
      {
        return UnprocessableEntity($"could not update exercise with given values.");
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await DeleteExerciseFromDbAsync(id);
      try
      {
        if (result)
        {
          return NoContent();
        }
        else
        {
          return NotFound($"no section exists with id {id}");
        }
      }
      catch (System.Exception)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError);
      }
    }

    private async Task<bool> DeleteExerciseFromDbAsync(int id)
    {
      if (id > 0)
      {
        using (IExerciseEntityModel context = ExerciseEntityModel.Create())
        {
          var exercise = await context.Exercises.SingleAsync(e => e.Id == id);
          if (exercise != null)
          {
            context.Exercises.Remove(exercise);
            await context.SaveChangesAsync();

            return true;
          }
          else
          {
            return false;
          }
        }
      }
      else
      {
        return false;
      }
    }

    private async Task<Models.Exercise> UpdateExerciseToDbAsync(Models.Exercise updatedExercise)
    {
      // validation?
      if (updatedExercise != null && updatedExercise.Id > 0)
      {
        using (IExerciseEntityModel context = ExerciseEntityModel.Create())
        {
          var dbExercise = context.Exercises
          .Single(e => e.Id == updatedExercise.Id);
          dbExercise.Name = updatedExercise.Name;
          dbExercise.StartTime = updatedExercise.StartTime;
          // we don't add sections when updating an exercise, that's adding a section!
          if (dbExercise.Sections == null)
          {
              dbExercise.Sections = new List<DataModels.Section>();
          }
          await context.SaveChangesAsync();
          return new Models.Exercise(dbExercise);
        }
      }
      else
      {
        return null;
      }
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
        if (exercise != null)
        {
          return new Models.Exercise(exercise);
        }
        else
        {
          return null;
        }
      }
    }

    private async Task<Models.Exercise> CreateExerciseToDbAsync(Models.Exercise newExercise)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        var newDbExercise = new DataModels.Exercise(newExercise);
        await context.Exercises.AddAsync(newDbExercise);

        await context.SaveChangesAsync();

        return new Models.Exercise(newDbExercise);
      }
    }
  }

}