using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumNUnitPOM.Pages;
using System;

namespace SeleniumNUnitPOM.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private const string BaseUrl = "http://automationexercise.com";

    // Test account credentials for login scenarios
    // Update these values with your own valid test account if needed
        private const string ValidEmail = "anh.bp@test.com";
        private const string ValidPassword = "anh.bp@test.com";
        private const string InvalidPassword = "wrongpassword";

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(BaseUrl);

            _loginPage = new LoginPage(_driver);
            Console.WriteLine($"[Setup] Browser launched at: {BaseUrl}");
        }

        [Test, Order(1)]
        public void LoginWithIncorrectCredentials_ShowsErrorMessage()
        {
            Console.WriteLine("[Ex1] Start: Login with incorrect credentials");

            Assert.That(_driver.Title.Contains("Automation Exercise"), "Home page is NOT visible.");

            _loginPage.ClickSignupLogin();
            Assert.IsTrue(_loginPage.IsLoginHeaderVisible(), "Login header is NOT visible.");

            _loginPage.EnterEmail(ValidEmail);
            _loginPage.EnterPassword(InvalidPassword);
            _loginPage.ClickLoginButton();

            Assert.IsTrue(_loginPage.IsErrorMessageVisible(), "Error message is NOT visible.");
            Console.WriteLine("[Ex1] PASSED: Error message displayed.");
        }

        [Test, Order(2)]
        public void LoginWithCorrectCredentials_ShowsLoggedInAsUsername()
        {
            Console.WriteLine("[Ex2] Start: Login with correct credentials");

            Assert.That(_driver.Title.Contains("Automation Exercise"), "Home page is NOT visible.");

            _loginPage.ClickSignupLogin();
            Assert.IsTrue(_loginPage.IsLoginHeaderVisible(), "Login header is NOT visible.");

            _loginPage.EnterEmail(ValidEmail);
            _loginPage.EnterPassword(ValidPassword);
            _loginPage.ClickLoginButton();

            Assert.IsTrue(_loginPage.IsLoggedInAsVisible(), "'Logged in as username' is NOT visible.");
            string username = _loginPage.GetLoggedInUsername();
            Console.WriteLine($"[Ex2] PASSED: Logged in as username '{username}' displayed.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            Console.WriteLine("[TearDown] Browser closed.");
        }
    }
}
