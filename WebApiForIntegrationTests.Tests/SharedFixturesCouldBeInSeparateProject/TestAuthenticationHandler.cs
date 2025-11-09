using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApiForIntegrationTests.Tests.SharedFixturesCouldBeInSeparateProject;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string TESTSCHEMENAME = "TestScheme";
    private readonly IGetTestUserService _userService;
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
                        ILoggerFactory logger, 
                        UrlEncoder encoder, 
                        ISystemClock clock,
                        IGetTestUserService getTestUserService) : base(options, logger, encoder, clock)
    {
        _userService = getTestUserService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal testUser = _userService.GetTestUser();
        AuthenticationTicket ticket = new AuthenticationTicket(testUser, TESTSCHEMENAME);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
