using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using treenirepository.Controllers;
using treenirepository.DataModels;
using Xunit;

namespace App.Tests
{
    public class SectionsControllerTests
    {
        private IExerciseEntityModel context;

        /// <summary>
        /// Initialize the test data in the constructor
        /// </summary>
        public SectionsControllerTests()
        {
            // can be used to initialize test context
            initalizeTestDb();
        }
        
        [Fact]
        public async Task TestContextInitialization()
        {
                // use the context
                var sections = await context.Sections.ToListAsync();
                Assert.Equal(sections.Count, 1);
        }

        [Fact]
        public async Task TestGet()
        {
            var ctrl = new SectionsController();

            var result = await ctrl.Get(1);

        }

        private void initalizeTestDb()
        {
            var options = new DbContextOptionsBuilder<ExerciseEntityModel>()
                .UseInMemoryDatabase(databaseName: "testingdata")
                .Options;

                context =  new ExerciseEntityModel(options);
                
                var ex = new Exercise{Name = "test exercise",  StartTime = DateTime.Now, Sections = new List<Section>() };
                ex.Sections.Add(new Section{Name = "Section 1", Description = "testing object", Duration = 10, SetupDuration = 2, Exercise = ex});
                context.Exercises.Add(
                    ex
                    );
                context.SaveChangesAsync();
        }
    }
}
