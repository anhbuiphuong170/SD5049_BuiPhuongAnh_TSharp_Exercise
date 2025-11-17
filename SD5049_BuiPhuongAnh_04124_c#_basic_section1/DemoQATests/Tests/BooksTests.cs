using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using DemoQATests.Pages;
using DemoQATests.Services;
using DemoQATests.Helpers;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading.Tasks;

namespace DemoQATests.Tests
{
    public class BooksTests : BaseTest
    {
        private ApiService api = null!;
        private const string Username = "buiphuonganh";
        private const string Password = "@Nh17102025";
        private const string TestTitle = "You Don't Know JS";

        [SetUp]
        public void InitApi() => api = new ApiService();

        [Test]
        public async Task VerifyDeleteBookAfterSearchAsync()
        {
            var (userId, token) = await api.LoginAsync(Username, Password);
            await api.AddAllBooksAsync(userId, token);

            var loginPage = new LoginPage(Driver);
            loginPage.NavigateTo("https://demoqa.com/login");
            loginPage.Login(Username, Password);

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("userName-value")));

            var profilePage = new ProfilePage(Driver);
            profilePage.NavigateTo("https://demoqa.com/profile");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("searchBox")));

            profilePage.SearchBook(TestTitle);
            bool found = await RetryHelper.RetryUntilAsync(() => profilePage.IsBookDisplayed(TestTitle));

            profilePage.DeleteBook(TestTitle);
            profilePage.SearchBook(TestTitle);
            bool deleted = await RetryHelper.RetryUntilAsync(() => !profilePage.IsBookDisplayed(TestTitle));

            Assert.Multiple(() =>
            {
                Assert.That(found, Is.True, $"[SEARCH] Book '{TestTitle}' should be visible after search.");
                Assert.That(deleted, Is.True, $"[DELETE] Book '{TestTitle}' should be removed and not visible.");
            });
            TestContext.WriteLine($"Book '{TestTitle}' was found and deleted successfully.");
        }
    }
}