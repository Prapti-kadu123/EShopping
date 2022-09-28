using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using EShopping.Controllers;
using FluentAssertions;
using EShopping.Models;

namespace EShopping.xUnitTestProject
{
    public partial class CategoriesApiTests
    {
        [Fact]
        public void GetCategories_OkResult()
        {
            // ARRANGE
            var dbName = nameof(CategoriesApiTests.GetCategories_OkResult);
            var logger = Mock.Of<ILogger<CategoriesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new CategoriesController(dbContext, logger);

            // ACT
            IActionResult actionResult = apiController.GetCategories().Result;

            // ASSERT
            // --- Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);

            // --- Check if the HTTP Response Code is 200 "Ok"
            int expectedStatusCode = (int)System.Net.HttpStatusCode.OK;
            int actualStatusCode = (actionResult as OkObjectResult).StatusCode.Value;
            Assert.Equal<int>(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public void GetCategories_CheckCorrectResult()
        {
            // ARRANGE
            var dbName = nameof(CategoriesApiTests.GetCategories_OkResult);
            var logger = Mock.Of<ILogger<CategoriesController>>();
            var dbContext = DbContextMocker.GetApplicationDbContext(dbName);
            var apiController = new CategoriesController(dbContext, logger);

            // ACT: Call the API action method
            IActionResult actionResult = apiController.GetCategories().Result;

            // ASSERT: Check if the ActionResult is of the type OkObjectResult
            Assert.IsType<OkObjectResult>(actionResult);


            // ACT: Extract the OkResult result 
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            // ASSERT: if the OkResult contains the object of the Correct Type
            Assert.IsAssignableFrom<List<Category>>(okResult.Value);


            // ACT: Extract the Categories from the result of the action
            var categoriesFromApi = okResult.Value.Should().BeAssignableTo<List<Category>>().Subject;

            // ASSERT: if the categories is NOT NULL
            Assert.NotNull(categoriesFromApi);

            // ASSERT: if the number of categories in the DbContext seed data
            //         is the same as the number of categories returned in the API Result
            Assert.Equal<int>(expected: DbContextMocker.TestData_Categories.Length,
                              actual: categoriesFromApi.Count);

            // ASSERT: Test the data received from the API against the Seed Data
            int ndx = 0;
            foreach (Category category in DbContextMocker.TestData_Categories)
            {
                // ASSERT: check if the Category ID is correct
                Assert.Equal<int>(expected: category.CategoryId,
                                  actual: categoriesFromApi[ndx].CategoryId);

                // ASSERT: check if the Category Name is correct
                Assert.Equal(expected: category.CategoryName,
                             actual: categoriesFromApi[ndx].CategoryName);

                _testOutputHelper.WriteLine($"Compared Row # {ndx} successfully");

                ndx++;          // now compare against the next element in the array
            }
        }
    }

}





