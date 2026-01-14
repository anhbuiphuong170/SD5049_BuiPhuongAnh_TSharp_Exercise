using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario05_DownloadPhotoTests : BaseTest
{
    [Test]
    public void Download_Photo_Success()
    {
        // 1. Log in
        new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

        // 2. Open a random photo
        var home = new HomePage(driver);
        home.Open();
        home.OpenFirstPhoto();

        var photoPage = new PhotoDetailPage(driver);
        
        // 3. Prepare for download verification
        var downloadsPath = DownloadHelper.GetDownloadsPath();
        var beforeFiles = DownloadHelper.SnapshotDownloads(downloadsPath);

        // 4. Download
        photoPage.Download();

        // 5. Wait for file to appear
        var downloadedFile = DownloadHelper.WaitForNewImage(beforeFiles, 20, downloadsPath);

        Assert.That(downloadedFile, Is.Not.Null, "File was not downloaded successfully");
        Assert.That(File.Exists(downloadedFile), Is.True, "Downloaded file does not exist");

        Unsplash.Automation.Tests.Utils.Logger.Debug($"Downloaded file: {downloadedFile}");

        // Cleanup
        DownloadHelper.DeleteFile(downloadedFile);
    }
}
