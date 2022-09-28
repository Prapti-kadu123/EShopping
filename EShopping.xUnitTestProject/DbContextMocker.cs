using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EShopping.Data;
using EShopping.Models;
using System;
using System.Collections.Generic;
using System.Text;
using EShopping.Web.Data;

namespace EShopping.xUnitTestProject
{
    public static class DbContextMocker
    {

        public static ApplicationDbContext GetApplicationDbContext(string dbName)
        {
            // Create a fresh service provider for the InMemory Database instance
            var serviceProvider = new ServiceCollection()
                                  .AddEntityFrameworkInMemoryDatabase()
                                  .BuildServiceProvider();

            // Create a new options instance telling the context
            // to use InMemory database with the new service provider created above
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseInMemoryDatabase(dbName)
                           .UseInternalServiceProvider(serviceProvider)
                           .Options;

            // Create the instance of the DbContext
            var dbContext = new ApplicationDbContext(options);

            // Add entities to the InMemory Database to seed the test data
            dbContext.SeedData();

            return dbContext;
        }


        // NOTE: InMemory Databases do not support Relationships, and Database Constraints properly.
        //       So, seed the Identity Column Values also.
        internal static readonly Category[] TestData_Categories
            = {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "First Category"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Second Category"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Third Category"
                },
            };


        /// <summary>
        ///     An extension Method for the DbContext object to Seed the Test Data.
        /// </summary>
        /// <param name="context">Application Db Context object.</param>
        private static void SeedData(this ApplicationDbContext context)
        {
            context.Categories.AddRange(TestData_Categories);

            // Commit the Changes to the database
            context.SaveChanges();
        }

    }
}
