using OpenQA.Selenium;

namespace SeleniumNUnitPOM.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ClickSignupLogin() => _driver.FindElement(By.XPath("//a[contains(text(),'Signup / Login')]")).Click();
        public bool IsLoginHeaderVisible() => _driver.FindElement(By.XPath("//h2[contains(text(),'Login to your account')]")).Displayed;
        public void EnterEmail(string email) => _driver.FindElement(By.XPath("//input[@data-qa='login-email']")).SendKeys(email);
        public void EnterPassword(string password) => _driver.FindElement(By.XPath("//input[@data-qa='login-password']")).SendKeys(password);
        public void ClickLoginButton() => _driver.FindElement(By.XPath("//button[@data-qa='login-button']")).Click();
        public bool IsErrorMessageVisible() => _driver.FindElement(By.XPath("//p[contains(text(),'Your email or password is incorrect!')]")).Displayed;
        public bool IsLoggedInAsVisible()
        {
            try
            {
                return _driver.FindElement(By.XPath("//a[contains(text(),'Logged in as')]")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
