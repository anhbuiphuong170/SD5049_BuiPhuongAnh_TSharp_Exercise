using Unsplash.Automation.Tests.Pages;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

[TestFixture]
    public class Scenario03_BookmarksTests : BaseTest
    {
    [Test]
    public void Bookmark_3_Random_Photos_Success()
    {
        new LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);

        var home = new HomePage(driver);
        var bookmarks = new BookmarksPage(driver);

        home.Open();

        bookmarks.ClickBookmarks();
        bookmarks.ClearAllBookmarks();

        home.Open();
        home.BookmarkRandomPhotos(3);

        bookmarks.ClickBookmarks();
        Assert.That(bookmarks.CountBookmarkedPhotos(), Is.EqualTo(3));
    }
    }