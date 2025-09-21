using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumNUnitPOM.Pages
{
    /// <summary>
    /// Page Object Model for the Login page of Automation Exercise.
    /// Encapsulates all interactions and verifications for login functionality.
    /// </summary>
    public class LoginPage
    {
        // WebDriver instance for browser interaction
        private readonly IWebDriver _driver;
        // WebDriverWait for explicit waits
        private readonly WebDriverWait _wait;

        /// <summary>
        /// Constructor initializes driver and wait.
        /// </summary>
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Page elements using explicit waits for reliability
        private IWebElement SignupLoginLink => 
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Signup / Login')]")));

        private IWebElement LoginHeader => 
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[contains(text(),'Login to your account')]")));

        private IWebElement EmailField => 
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-qa='login-email']")));

        private IWebElement PasswordField => 
            _driver.FindElement(By.XPath("//input[@data-qa='login-password']"));

        private IWebElement LoginButton => 
            _driver.FindElement(By.XPath("//button[@data-qa='login-button']"));

        private IWebElement ErrorMessage => 
            _driver.FindElement(By.XPath("//p[contains(text(),'Your email or password is incorrect!')]"));

        private IWebElement LoggedInAs => 
            _driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]"));

        /// <summary>
        /// Clicks the Signup/Login link on the home page.
        /// </summary>
        public void ClickSignupLogin() => SignupLoginLink.Click();

        /// <summary>
        /// Checks if the login header is visible.
        /// </summary>
        public bool IsLoginHeaderVisible() => LoginHeader.Displayed;

        /// <summary>
        /// Enters the email address in the login form.
        /// </summary>
        public void EnterEmail(string email) => EmailField.SendKeys(email);

        /// <summary>
        /// Enters the password in the login form.
        /// </summary>
        public void EnterPassword(string password) => PasswordField.SendKeys(password);

        /// <summary>
        /// Clicks the login button.
        /// </summary>
        public void ClickLoginButton() => LoginButton.Click();

        /// <summary>
        /// Checks if the error message for invalid login is visible.
        /// </summary>
        public bool IsErrorMessageVisible() => ErrorMessage.Displayed;

        /// <summary>
        /// Checks if the 'Logged in as' message is visible after successful login.
        /// </summary>
        public bool IsLoggedInAsVisible()
        {
            try
            {
                return LoggedInAs.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the specific username from the 'Logged in as' element.
        /// </summary>
        public string GetLoggedInUsername()
        {
            try
            {
                var text = LoggedInAs.Text;
                // Assumes format: 'Logged in as <username>'
                var parts = text.Split(' ');
                if (parts.Length >= 4)
                    return parts[3];
                return string.Empty;
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }
    }
}
