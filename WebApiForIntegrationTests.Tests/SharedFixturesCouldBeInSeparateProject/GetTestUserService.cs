using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiForIntegrationTests.Tests.SharedFixturesCouldBeInSeparateProject;

public interface IGetTestUserService
{
    ClaimsPrincipal GetTestUser();
}
public class GetTestUserService : IGetTestUserService
{
    public readonly ClaimsPrincipal _testUser;
    public GetTestUserService(ClaimsPrincipal testUser)
    {
        _testUser = testUser;
    }
    public ClaimsPrincipal GetTestUser()
    {
        return _testUser;
    }
}
