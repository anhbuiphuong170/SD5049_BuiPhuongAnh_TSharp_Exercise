using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumNUnitPOM.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

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

        public void ClickSignupLogin() => SignupLoginLink.Click();
        public bool IsLoginHeaderVisible() => LoginHeader.Displayed;
        public void EnterEmail(string email) => EmailField.SendKeys(email);
        public void EnterPassword(string password) => PasswordField.SendKeys(password);
        public void ClickLoginButton() => LoginButton.Click();
        public bool IsErrorMessageVisible() => ErrorMessage.Displayed;

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
        // Returns the specific username from the 'Logged in as' element
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
