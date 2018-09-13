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

  /// <summary>
  /// Controller for Section database manipulation.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class SectionsController : Controller
  {
    /// <summary>
    /// Get a single section from the database by its Id.
    /// </summary>
    /// <param name="id">Id of section.</param>
    /// <returns>Single <see cref="Models.Section"/>.</returns>
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

    /// <summary>
    /// Create a new section to database.
    /// </summary>
    /// <param name="newSection">new <see cref="Models.Section"/> object.</param>
    /// <returns><see cref="Models.Section"/> with database id.</returns>
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

    /// <summary>
    /// Update a section in the database.
    /// </summary>
    /// <param name="updatedSection">modified <see cref="Models.Section"/> object.</param>
    /// <returns>updated <see cref="Models.Section"/>.</returns>
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

    /// <summary>
    /// Delete a section by its id.
    /// </summary>
    /// <param name="id">Id of the section to be deleted.</param>
    /// <returns>204 no content http response.</returns>
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
          var dataSection = context.Sections
          .Single(s => s.Id == updatedSection.Id);
          dataSection.Name = updatedSection.Name;
          dataSection.Duration = updatedSection.Duration;
          dataSection.Color = updatedSection.Color;
          dataSection.SetupDuration = updatedSection.SetupDuration;

          await context.SaveChangesAsync();
          return new Models.Section(dataSection);
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

        if (!context.Exercises.Any(e => e.Id == newDbSection.ExerciseId))
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