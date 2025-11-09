using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiForIntegrationTests.Tests.SharedFixturesCouldBeInSeparateProject;

public class TestUsersFixture
{
    public readonly List<ClaimsPrincipal> TestUsers = new();

    public TestUsersFixture()
    {
        LoadUsers();
    }

    private void LoadUsers()
    {
        Claim name = new Claim(ClaimTypes.Name, "Marco");
        Claim email = new Claim(ClaimTypes.Email, "marco@marco.com");
        Claim admin = new Claim(ClaimTypes.Role, "Admin");
        Claim shoeSize = new Claim("shoeSize", "43");

        ClaimsIdentity marcoIdentity = new([name, email, admin, shoeSize]);

        ClaimsPrincipal marco = new ClaimsPrincipal(marcoIdentity);
        TestUsers.Add(marco);
    }
}
