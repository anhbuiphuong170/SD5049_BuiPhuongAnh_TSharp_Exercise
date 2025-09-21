using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // for ExpectedConditions

namespace SeleniumAutomationExercise
{
    class Program
    {
        private static IWebDriver? driver;   
        private static WebDriverWait? wait;  

        // Centralize locators
        private static readonly By signupLoginBtn = By.XPath("//a[contains(text(),'Signup / Login')]");
        private static readonly By loginHeader = By.XPath("//h2[contains(text(),'Login to your account')]");
        private static readonly By loginEmail = By.XPath("//input[@data-qa='login-email']");
        private static readonly By loginPassword = By.XPath("//input[@data-qa='login-password']");
        private static readonly By loginButton = By.XPath("//button[@data-qa='login-button']");
        private static readonly By errorMessage = By.XPath("//p[contains(text(),'Your email or password is incorrect!')]");
        private static readonly By loggedInMsg = By.XPath("//a[contains(text(),'Logged in as')]");

        // Centralized credentials
        private static readonly string validEmail = "anh.bp@test.com";
        private static readonly string validPassword = "anh.bp@test.com";
        private static readonly string invalidEmail = "wrongemail@test.com";
        private static readonly string invalidPassword = "wrongpassword";

        static void Main(string[] args)
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                driver.Manage().Window.Maximize();
                RunInvalidLoginTest();
                RunValidLoginTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Test crashed with exception: {ex.Message}");
            }
            finally
            {
                driver?.Quit();
                Console.WriteLine("\nBrowser closed. Test finished.");
            }
        }

        private static void RunInvalidLoginTest()
        {
            Console.WriteLine("\n*** Exercise 1: Invalid Login ***");

            NavigateToHomePage();
            ClickAndVerifyLoginHeader();

            // Use invalid credentials
            driver!.FindElement(loginEmail).SendKeys(invalidEmail);
            driver.FindElement(loginPassword).SendKeys(invalidPassword);
            driver.FindElement(loginButton).Click();

            if (IsElementVisible(errorMessage))
            {
                var errorMsg = driver.FindElement(errorMessage);
                Console.WriteLine("[PASS] Error message is visible: " + errorMsg.Text);
            }
            else
            {
                Console.WriteLine("[FAIL] Error message not found");
            }
        }

        private static void RunValidLoginTest()
        {
            Console.WriteLine("\n*** Exercise 2: Valid Login ***");

            NavigateToHomePage();
            ClickAndVerifyLoginHeader();

            // Use valid credentials
            driver!.FindElement(loginEmail).SendKeys(validEmail);
            driver.FindElement(loginPassword).SendKeys(validPassword);
            driver.FindElement(loginButton).Click();

            if (IsElementVisible(loggedInMsg))
            {
                var loggedInElement = driver.FindElement(loggedInMsg);
                Console.WriteLine("[PASS] Logged in as is visible: " + loggedInElement.Text);
            }
            else
            {
                Console.WriteLine("[FAIL] 'Logged in as' not found");
            }
        }

        // --- Helpers ---
        private static void NavigateToHomePage()
        {
            driver!.Navigate().GoToUrl("http://automationexercise.com");
            if (driver.Title.Contains("Automation Exercise"))
                Console.WriteLine("[PASS] Home page is visible");
            else
                Console.WriteLine("[FAIL] Home page NOT visible");
        }

        private static void ClickAndVerifyLoginHeader()
        {
            driver!.FindElement(signupLoginBtn).Click();
            if (IsElementVisible(loginHeader))
                Console.WriteLine("[PASS] 'Login to your account' is visible");
            else
                Console.WriteLine("[FAIL] Login header not found");
        }

        private static bool IsElementVisible(By locator)
        {
            try
            {
                var element = wait!.Until(ExpectedConditions.ElementIsVisible(locator));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
