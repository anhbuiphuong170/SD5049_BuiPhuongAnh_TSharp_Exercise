using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;
using NUnit.Framework;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
public class Scenario04_CollectionsTests : BaseTest
{
    [Test]
public async Task Remove_Photo_From_Collection_Success()
{
    new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

    var home = new HomePage(driver);
    var collections = new CollectionsPage(driver);

    home.Open();

    string collectionName = "BPA_" + DateTime.Now.Ticks;

    // ✅ KHÔNG GÁN RETURN
    home.CreateCollectionAndAddPhotos(collectionName, 2);

    collections.OpenCollections();
    collections.OpenCollection(collectionName);

    Assert.That(collections.CountPhotos(), Is.EqualTo(2));

    // ✅ LẤY collectionId TỪ URL
    string collectionId = collections.GetCurrentCollectionId();

    string cookies = string.Join("; ",
        driver.Manage().Cookies.AllCookies
            .Select(c => $"{c.Name}={c.Value}")
    );

    var api = new UnsplashApiClient(cookies);
    string photoId = collections.GetFirstPhotoId();

    await api.RemovePhoto(collectionId, photoId);

    collections.OpenCollections();
    collections.OpenCollection(collectionName);

    Assert.That(collections.CountPhotos(), Is.EqualTo(1));
}

}
