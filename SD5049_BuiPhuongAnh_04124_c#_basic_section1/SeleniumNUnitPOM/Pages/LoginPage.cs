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

        // Prefer id/name first, fallback to xpath
        private By SignupLoginLink => By.XPath("//a[contains(text(),'Signup / Login')]");
        private By LoginHeader => By.XPath("//h2[contains(text(),'Login to your account')]");
        private By EmailField => By.Name("email");
        private By PasswordField => By.Name("password");
        private By LoginButton => By.XPath("//button[@data-qa='login-button']");
        private By ErrorMessage => By.XPath("//p[contains(text(),'Your email or password is incorrect!')]");
        private By LoggedInAs => By.XPath("//a[contains(text(),'Logged in as')]");

        public void ClickSignupLogin()
        {
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(SignupLoginLink));
            element.Click();
        }

        public bool IsLoginHeaderVisible()
        {
            try
            {
                var element = _wait.Until(ExpectedConditions.ElementIsVisible(LoginHeader));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void EnterEmail(string email)
        {
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(EmailField));
            element.Clear();
            element.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            var element = _wait.Until(ExpectedConditions.ElementIsVisible(PasswordField));
            element.Clear();
            element.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var button = _wait.Until(ExpectedConditions.ElementToBeClickable(LoginButton));
            button.Click();
        }

        public bool IsErrorMessageVisible()
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(ErrorMessage)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsLoggedInAsVisible()
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(LoggedInAs)).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public string GetLoggedInUsername()
        {
            try
            {
                var element = _wait.Until(ExpectedConditions.ElementIsVisible(LoggedInAs));
                var text = element.Text;
                var parts = text.Split(' ');
                return parts.Length >= 4 ? parts[3] : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
