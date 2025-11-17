using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DemoQATests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By usernameField = By.Id("userName");
        private readonly By passwordField = By.Id("password");
        private readonly By loginButton = By.Id("login");
        private readonly By userNameValue = By.Id("userName-value");

        private const int DefaultTimeout = 20;

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Login(string username, string password)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultTimeout));

            wait.Until(ExpectedConditions.ElementIsVisible(usernameField)).SendKeys(username);
            wait.Until(ExpectedConditions.ElementIsVisible(passwordField)).SendKeys(password);

            var loginBtn = wait.Until(ExpectedConditions.ElementToBeClickable(loginButton));
            ScrollIntoView(loginBtn);

            TryClickWithRetries(loginBtn);

            WaitForLoginConfirmation();
        }

        private void TryClickWithRetries(IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            int attempts = 0;
            const int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                attempts++;
                try
                {
                    RemoveAdOverlays(js);
                    ScrollIntoView(element);

                    try
                    {
                        element.Click();
                        return;
                    }
                    catch (ElementClickInterceptedException)
                    {
                        js.ExecuteScript("arguments[0].click();", element);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Login click attempt {attempts} failed: {ex.Message}");
                }

                System.Threading.Thread.Sleep(500);
            }

            throw new ElementClickInterceptedException("Login button could not be clicked after retries.");
        }

        private void WaitForLoginConfirmation()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultTimeout));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(userNameValue));
            }
            catch
            {
                try { wait.Until(ExpectedConditions.ElementIsVisible(userNameValue)); }
                catch (Exception ex)
                {
                    Console.WriteLine($"[WARN] Ignored exception: {ex.Message}");
                }
            }
        }

        private void ScrollIntoView(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        private void RemoveAdOverlays(IJavaScriptExecutor js)
        {
            try
            {
                js.ExecuteScript(@"
                    document.querySelectorAll('iframe[id^=""google_ads_iframe""], iframe[src*=""ads""], iframe[src*=""googlesyndication""]').forEach(e => e.remove());
                    document.querySelectorAll('[class*=""ad""], [id*=""ad""], [aria-label*=""Advertisement""]').forEach(e => {
                        e.style.display = 'none';
                        e.style.pointerEvents = 'none';
                    });
                ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ad removal warning: {ex.Message}");
            }
        }

        public bool IsLoggedIn()
        {
            try
            {
                var userEl = driver.FindElement(userNameValue);
                return userEl.Displayed && !string.IsNullOrEmpty(userEl.Text);
            }
            catch { return false; }
        }

        public void LogoutIfNeeded()
        {
            if (!IsLoggedIn()) return;

            NavigateTo("https://demoqa.com/profile");

            try
            {
                var logoutBtn = driver.FindElement(By.XPath("//button[text()='Log out']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", logoutBtn);
                System.Threading.Thread.Sleep(1000);
            }
            catch
            {
                Console.WriteLine("Logout failed or button not found.");
            }
        }
    }
}