
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
        public async Task<IActionResult> CreateExerciseAsync([FromBody] Models.Section newSection)
        {
            try
            {
                return Ok(await CreateSectionToDbAsync(newSection));
            }
            catch(Exception e)
            {
                // TODO: check whick error code is actually preferable in this case.
                return UnprocessableEntity($"could not create database entity from {newSection} \n details: {e.StackTrace}");
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

        private async Task<IEnumerable<Models.Section>> CreateSectionToDbAsync(Models.Section newSection)
        {
            using (IExerciseEntityModel context = ExerciseEntityModel.Create())
            {
                await context.Sections.AddAsync(new DataModels.Section(newSection));

                await context.SaveChangesAsync();

            }
            return newSection as IEnumerable<Models.Section>;
        }
    }

}