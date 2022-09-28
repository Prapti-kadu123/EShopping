using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace EShopping.xUnitTestProject
{
    // Add the following NuGet Packages:
    //      Microsoft.EntityFrameworkCore.InMemory      (same version as EF Core in EShopping)
    //      Moq                                         (latest version)
    //      FluentAssertions                            (latest version)
    // Also add Project Reference to the EShopping

    public partial class CategoriesApiTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CategoriesApiTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

    }
}
