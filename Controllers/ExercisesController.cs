
namespace treenirepository.Controllers
{

  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using treenirepository.DataModels;

  [Route("api/[controller]")]
  public class ExercisesController : Controller
  {
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Exercise>), (int)HttpStatusCode.OK)]
    public async Task<IEnumerable<Models.Exercise>> Get()
    {

      return await GetExercisesFromDbAsync();
    }

    [HttpPost]
    [Route("create")]
    [ProducesResponseType(typeof(IEnumerable<Models.Exercise>), (int)HttpStatusCode.OK)]
    public async Task<IEnumerable<Models.Exercise>> CreateExerciseAsync([FromBody] Models.Exercise newExercise)
    {
      return await CreateExerciseToDbAsync(newExercise);
    }

    private async Task<IEnumerable<Models.Exercise>> GetExercisesFromDbAsync()
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        // db operation as async
        var exercises = await context.Exercises
          .AsNoTracking()
          .ToListAsync();

        // return formatting
        return exercises.Select(e =>
          new Models.Exercise(e));
      }
    }

    private async Task<IEnumerable<Models.Exercise>> CreateExerciseToDbAsync(Models.Exercise newExercise)
    {
      using (IExerciseEntityModel context = ExerciseEntityModel.Create())
      {
        await context.Exercises.AddAsync(new Exercise(newExercise));
        
        await context.SaveChangesAsync();

      }
      return newExercise as IEnumerable<Models.Exercise>;
    }
  }

}