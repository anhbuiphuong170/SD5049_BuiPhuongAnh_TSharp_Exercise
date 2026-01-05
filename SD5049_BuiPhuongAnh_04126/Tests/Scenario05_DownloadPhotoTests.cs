using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario05_DownloadPhotoTests : BaseTest
{
    [Test]
    public void Download_Photo_Success()
    {
        new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

        new HomePage(driver).OpenFirstPhoto();
        new PhotoDetailPage(driver).Download();

        var path = Path.Combine(
            Directory.GetCurrentDirectory(), "Downloads");

        Assert.That(Directory.GetFiles(path).Length, Is.GreaterThan(0));
    }
}
