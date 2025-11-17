using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DemoQATests.Helpers
{
    public static class AlertHelper
    {
        public static void AcceptAlertIfPresent(IWebDriver driver, int timeoutSeconds = 3)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.AlertIsPresent());
                driver.SwitchTo().Alert().Accept();
            }
            catch (WebDriverTimeoutException) { }
            catch (NoAlertPresentException) { }
        }
    }
}