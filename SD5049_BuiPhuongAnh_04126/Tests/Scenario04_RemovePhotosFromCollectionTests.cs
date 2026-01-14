using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario04_RemovePhotosFromCollectionTests : BaseTest
{
    // Setup is handled by BaseTest and in-test Login
    [Test]
    public async Task Remove_Photo_From_Collection_Success()
    {
        string? collectionId = null;
        UnsplashApiClient? apiClient = null;

        try
        {
            // 1. Log in
            new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

            // Init API client for teardown via factory
            apiClient = ApiClientFactory.CreateFromDriver(driver);

            // 2. Create private collection & Add photos via UI
            var title = "BPA_" + DateTime.Now.Ticks.ToString()[^6..];
            var home = new HomePage(driver);
            home.Open();

            // Add 2 photos (creates collection on first, adds to it on second)
            var (createdId, collectionDetail) = home.CreateCollectionAndAddPhotos(title, 2);
            collectionId = createdId;

            // Verify we see 2 photos initially (wait for sync)
            int initialCount = collectionDetail.CountPhotos();
            Assert.That(initialCount, Is.GreaterThanOrEqualTo(2), "Initial photo count should be at least 2");

            var removed = collectionDetail.RemoveAPhotoFromCollection(title);
            Assert.That(removed, Is.True, "Failed to remove a photo from the collection");

            // 5. Verify removal - refresh to be sure
            collectionDetail.RefreshPage();
            var finalCount = collectionDetail.CountPhotos();
            Assert.That(finalCount, Is.LessThan(initialCount), "Photo count should decrease (related photos may also disappear)");
            Assert.That(finalCount, Is.GreaterThanOrEqualTo(1), "Should still have at least 1 photo left");
        }
        finally
        {
            if (!string.IsNullOrEmpty(collectionId) && apiClient != null)
            {
                await CleanupHelper.DeleteCollectionAsync(apiClient, collectionId, driver);
            }
        }
    }
}
