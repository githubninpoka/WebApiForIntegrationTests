using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiForIntegrationTests.Tests.SharedFixturesCouldBeInSeparateProject;

namespace WebApiForIntegrationTests.Tests.Controllers;

public class WeatherForecastControllerTests : 
        IClassFixture<CustomWebApplicationFactoryFixture>, 
        IClassFixture<TestUsersFixture>
{
    private readonly CustomWebApplicationFactoryFixture _webApplicationFixture;
    private readonly TestUsersFixture _testUsersFixture;
    private readonly ITestOutputHelper _testOutputHelper;
    public WeatherForecastControllerTests(
        CustomWebApplicationFactoryFixture customWebApplicationFactoryFixture,
        TestUsersFixture testUsersFixture,
        ITestOutputHelper testOutputHelper)
    {
        _webApplicationFixture = customWebApplicationFactoryFixture;
        _testUsersFixture = testUsersFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetWeatherForecast_return200Ok_WhenAuthenticated()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = _testUsersFixture.TestUsers
            .First(x => x.Claims.Any(x => x.Type == ClaimTypes.Name ) 
                        && x.Identity.Name == "Marco"
                        //&& x.Claims.Contains(aSingleClaim => aSingleClaim.Value == "marco"
            );
        _webApplicationFixture.SetTestUser(claimsPrincipal);
        var client = _webApplicationFixture.CreateClient();

        // Act
        var result = await client.GetAsync("WeatherForecast");
        _testOutputHelper.WriteLine(await result.Content.ReadAsStringAsync());

        // Assert
        Assert.True(result.StatusCode == System.Net.HttpStatusCode.OK);
    }
}
