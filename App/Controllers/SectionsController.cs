
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
  public class SectionsController : Controller
  {
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Models.Section), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        return Ok(await GetSectionFromDbAsync(id));
      }
      catch
      {
        return NotFound($"no section exists with id {id}");
      }
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(IEnumerable<Models.Section>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateSectionAsync([FromBody] Models.Section newSection)
    {
      var result = await CreateSectionToDbAsync(newSection);
      if(result != null)
      {
        return Ok(result);
      }
      else
      {
          return BadRequest("Could not save section.");
      }
    }

    [HttpPost]
    [Route("update")]
    [ProducesResponseType(typeof(IEnumerable<Models.Section>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateSectionAsync([FromBody] Models.Section updatedSection)
    {
      try
      {
        return Ok(await UpdateSectionToDbAsync(updatedSection));
      }
      catch (System.Exception)
      {
        return BadRequest("Could not link the section to the given exercise.");
      }

    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var result = await DeleteSectionFromDbAsync(id);
        if (result)
        {
          return NoContent();
        }
        else
        {
          return NotFound($"no section exists with id {id}");
        }
      }
      catch (Exception)
      {
        // log stack?
        return StatusCode((int)HttpStatusCode.InternalServerError);
      }
    }

    private async Task<bool> DeleteSectionFromDbAsync(int id)
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

    private async Task<Models.Section> UpdateSectionToDbAsync(Models.Section updatedSection)
    {
      if (updatedSection != null && updatedSection.Id > 0)
      {
        using (IExerciseEntityModel context = ExerciseEntityModel.Create())
        {
          var dbSection = context.Sections
          .Single(s => s.Id == updatedSection.Id);
          dbSection.Name = updatedSection.Name;
          dbSection.Duration = updatedSection.Duration;
          dbSection.Color = updatedSection.Color;
          dbSection.SetupDuration = updatedSection.SetupDuration;

          await context.SaveChangesAsync();
          return new Models.Section(dbSection);
        }
      }
      else
      {
        return null;
      }
    }

    private async Task<Models.Section> GetSectionFromDbAsync(int id)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        var section = await context.Sections.AsNoTracking().SingleAsync(s => s.Id == id);
        return new Models.Section(section);
      }
    }

    private async Task<Models.Section> CreateSectionToDbAsync(Models.Section newSection)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        var newDbSection = new DataModels.Section(newSection);
        newDbSection.Id = 0;

        if (context.Exercises.Any(e => e.Id == newDbSection.ExerciseId))
        {
          return null;
        }

        await context.Sections.AddAsync(newDbSection);

        await context.SaveChangesAsync();

        return new Models.Section(newDbSection);
      }
    }
  }

}