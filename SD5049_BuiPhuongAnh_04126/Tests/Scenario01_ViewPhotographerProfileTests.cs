using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario01_ViewPhotographerProfileTests : BaseTest
{
[Test]
public void View_Photographer_Profile_Success()
{
    new LoginPage(driver)
        .Login(TestConfig.Email, TestConfig.Password);

    var home = new HomePage(driver);
    home.Open();
    home.OpenFirstPhoto();

    var photo = new PhotoDetailPage(driver);
    photo.ViewPhotographerProfile();
    // Verify redirect to photographer profile page
    Assert.That(driver.Url, Does.Contain("/@"),
        "Do not redirect to the photographer profile page.");
}

}
