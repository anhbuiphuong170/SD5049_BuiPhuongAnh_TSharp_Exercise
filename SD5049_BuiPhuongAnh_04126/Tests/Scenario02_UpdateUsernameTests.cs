using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Pages.Components;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario02_UpdateUsernameTests : BaseTest
{
    [Test]
    public void Update_Username_And_View_Profile_Success()
    {
        new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

        new UserHeader(driver).GoToMyProfile();
            
        var profilePage = new ProfilePage(driver);
        var newUsername = profilePage.UpdateUsername(TestConfig.Username);

        profilePage.OpenProfileByUsername(newUsername);

        // Verify full name
        var fullName = profilePage.GetFullName();
        Assert.That(fullName, Is.EqualTo("Anh Bui Phuong"));
    }
}
