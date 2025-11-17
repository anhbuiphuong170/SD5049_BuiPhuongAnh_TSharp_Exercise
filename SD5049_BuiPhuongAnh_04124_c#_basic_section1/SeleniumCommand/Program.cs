using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumAutomationExercise
{
    internal class Program
    {
        private IWebDriver? _driver;
        private WebDriverWait? _wait;

        private readonly By _signupLoginBtn = By.XPath("//a[contains(text(),'Signup / Login')]");
        private readonly By _loginHeader = By.XPath("//h2[contains(text(),'Login to your account')]");
        private readonly By _loginEmail = By.Name("email");
        private readonly By _loginPassword = By.Name("password");
        private readonly By _loginButton = By.XPath("//button[@data-qa='login-button']");
        private readonly By _errorMessage = By.XPath("//p[contains(text(),'Your email or password is incorrect!')]");
        private readonly By _loggedInMsg = By.XPath("//a[contains(text(),'Logged in as')]");

        private const string ValidEmail = "anh.bp@test.com";
        private const string ValidPassword = "anh.bp@test.com";
        private const string InvalidEmail = "wrongemail@test.com";
        private const string InvalidPassword = "wrongpassword";

        static void Main(string[] args)
        {
            var program = new Program();
            program.RunTests();
        }

        private void RunTests()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                _driver.Manage().Window.Maximize();

                RunInvalidLoginTest();
                RunValidLoginTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Test crashed: {ex.Message}");
            }
            finally
            {
                _driver?.Quit();
                Console.WriteLine("\nBrowser closed. Test finished.");
            }
        }

        //  Test 1: Invalid login
        private void RunInvalidLoginTest()
        {
            Console.WriteLine("\n=== [Ex1] Invalid Login Test ===");
            NavigateToHomePage();
            ClickLoginPage();

            Console.WriteLine("Step: Enter invalid credentials");
            _driver!.FindElement(_loginEmail).SendKeys(InvalidEmail);
            _driver.FindElement(_loginPassword).SendKeys(InvalidPassword);
            _driver.FindElement(_loginButton).Click();

            //  Final assertion only
            bool result = IsElementVisible(_errorMessage);
            Console.WriteLine(result
                ? "[PASS] Error message displayed as expected."
                : "[FAIL] Error message NOT displayed.");
        }

        //  Test 2: Valid login
        private void RunValidLoginTest()
        {
            Console.WriteLine("\n=== [Ex2] Valid Login Test ===");
            NavigateToHomePage();
            ClickLoginPage();

            Console.WriteLine("Step: Enter valid credentials");
            _driver!.FindElement(_loginEmail).SendKeys(ValidEmail);
            _driver.FindElement(_loginPassword).SendKeys(ValidPassword);
            _driver.FindElement(_loginButton).Click();

            //  Final assertion only
            bool result = IsElementVisible(_loggedInMsg);
            Console.WriteLine(result
                ? "[PASS] 'Logged in as' displayed correctly."
                : "[FAIL] Login failed — 'Logged in as' not found.");
        }

        // Helper methods
        private void NavigateToHomePage()
        {
            _driver!.Navigate().GoToUrl("http://automationexercise.com");
            Console.WriteLine("Step: Navigated to home page.");
        }

        private void ClickLoginPage()
        {
            _driver!.FindElement(_signupLoginBtn).Click();
            if (IsElementVisible(_loginHeader))
                Console.WriteLine("Step: Open login page.");
            else
                Console.WriteLine("Login page not found.");
        }

        private bool IsElementVisible(By locator)
        {
            try
            {
                var element = _wait!.Until(ExpectedConditions.ElementIsVisible(locator));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
