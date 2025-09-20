using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumNUnitPOM.Pages;

namespace SeleniumNUnitPOM.Tests
{
    public class LoginTests
    {
        private IWebDriver _driver;
        private LoginPage _loginPage;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("http://automationexercise.com");
            _loginPage = new LoginPage(_driver);
        }

        [Test, Order(1)]
        public void LoginWithIncorrectCredentials_ShowsErrorMessage()
        {
            Console.WriteLine("[Ex1] Start: Login with incorrect credentials");
            Assert.That(_driver.Title.Contains("Automation Exercise"), "Home page is NOT visible.");
            Console.WriteLine("Home page is visible.");
            _loginPage.ClickSignupLogin();
            Console.WriteLine("Clicked 'Signup / Login'.");
            Assert.IsTrue(_loginPage.IsLoginHeaderVisible(), "Login header is NOT visible.");
            Console.WriteLine("Login header is visible.");
            _loginPage.EnterEmail("wrongemail@test.com");
            _loginPage.EnterPassword("wrongpassword");
            Console.WriteLine("Entered incorrect email and password.");
            _loginPage.ClickLoginButton();
            Console.WriteLine("Clicked login button.");
            Assert.IsTrue(_loginPage.IsErrorMessageVisible(), "Error message is NOT visible.");
            Console.WriteLine("Error message is visible: Test PASSED.");
        }

        [Test, Order(2)]
        public void LoginWithCorrectCredentials_ShowsLoggedInAsUsername()
        {
            Console.WriteLine("[Ex2] Start: Login with correct credentials");
            Assert.That(_driver.Title.Contains("Automation Exercise"), "Home page is NOT visible.");
            Console.WriteLine("Home page is visible.");
            _loginPage.ClickSignupLogin();
            Console.WriteLine("Clicked 'Signup / Login'.");
            Assert.IsTrue(_loginPage.IsLoginHeaderVisible(), "Login header is NOT visible.");
            Console.WriteLine("Login header is visible.");
            string correctEmail = "anh.bp@test.com"; // Replace with valid email
            string correctPassword = "anh.bp@test.com";    // Replace with valid password
            _loginPage.EnterEmail(correctEmail);
            _loginPage.EnterPassword(correctPassword);
            Console.WriteLine("Entered correct email and password.");
            _loginPage.ClickLoginButton();
            Console.WriteLine("Clicked login button.");
            Assert.IsTrue(_loginPage.IsLoggedInAsVisible(), "'Logged in as username' is NOT visible.");
            Console.WriteLine("'Logged in as username' is visible: Test PASSED.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
