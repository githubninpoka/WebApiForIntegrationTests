using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiForIntegrationTests.Tests.SharedFixturesCouldBeInSeparateProject;

public class CustomWebApplicationFactoryFixture : WebApplicationFactory<WebApiForIntegrationTests.Program>
{
    private ClaimsPrincipal _testUser = null;
    
    public void SetTestUser(ClaimsPrincipal testUser)
    {
        _testUser = testUser;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //base.ConfigureWebHost(builder); // not needed
        
        // you can set an environment here.
        // you could have your 'normal' host respond differently.
        // for example if environment is testing, it would not add dbcontext at all, or a different one.
        // in this case it will cause swagger not to be loaded.
        builder.UseEnvironment("Testing");

        // ConfigureTestServices leaves in place all services that are already in the real Host
        // this method runs AFTER the real Host is configured.
        // So this method is useful for removing the DbContext and replacing it with a new one..        
        builder.ConfigureTestServices(services => {

            // this service is so that the TestAuthenticationHandler can use it with
            // dependency injection to get the claims principal for which it will authenticate.
            services.AddSingleton<IGetTestUserService>(new GetTestUserService(_testUser));

            // here for demo we strip existing authentication schemes. normally you would leave them be.
            var alreadyExistingAuthenticationSchemes = services
                                .Where(descriptor => descriptor.ServiceType == typeof(IAuthenticationService));
            if (alreadyExistingAuthenticationSchemes.Any())
            {
                foreach (var authenticationScheme in alreadyExistingAuthenticationSchemes)
                {
                    //services.Remove(authenticationScheme);
                }
            }

            // add a new authenticationhandler for testing purposes
            services.AddAuthentication(TestAuthenticationHandler.TESTSCHEMENAME)
            .AddScheme<AuthenticationSchemeOptions,TestAuthenticationHandler>(TestAuthenticationHandler.TESTSCHEMENAME,null);
        });
    }

}
