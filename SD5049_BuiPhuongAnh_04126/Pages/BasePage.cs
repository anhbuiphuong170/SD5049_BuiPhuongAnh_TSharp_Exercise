using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Pages;

public abstract class BasePage
{
    // BasePage: centralizes common page behaviours and helpers used by all page objects.
    // - Holds the shared `IWebDriver` and `WebDriverWait` instances.
    // - Provides stable actions like Click, Hover, ScrollIntoView and refresh helpers.
    // Page objects should inherit from this class and use these helpers instead of raw WebDriver calls.
    protected IWebDriver driver;
    protected WebDriverWait wait;
    protected By dialogLocator = By.XPath("//div[@role='dialog']");
    protected By optionLocator = By.XPath("//div[@role='option']");
    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultTimeoutSeconds));
    }
    protected IWebElement Visible(By locator)
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    /// <summary>
    /// Click a clickable element identified by <paramref name="locator"/>.
    /// This helper waits until clickable, scrolls the element into view and falls back to JS click
    /// when a normal click is intercepted. Use this instead of calling Click() on IWebElement directly.
    /// </summary>
    protected void Click(By locator)
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        // Wait until element is displayed after scrolling
        wait.Until(d =>
        {
            try { return element.Displayed; } catch { return false; }
        });
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", element);
        }
    }
    protected void Hover(By locator)
    {
        var element = Visible(locator);
        new Actions(driver).MoveToElement(element).Perform();
    }
    /// <summary>
    /// Hover over an element reference. Use when the element is already located (IWebElement).
    /// </summary>
    protected void Hover(IWebElement element)
    {
        new Actions(driver).MoveToElement(element).Perform();
    }
    protected void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
    }
    protected void JsClick(IWebElement element)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
    }
    protected void WaitPageStable()
    {
        TestConfig.Pause();
    }
    protected void RefreshAndWait()
    {
        driver.Navigate().Refresh();

        wait.Until(d =>
        {
            var readyState = ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState");
            return readyState != null && readyState.ToString() == "complete";
        });

        TestConfig.Pause();
    }
    public void RefreshPage()
    {
        RefreshAndWait();
    }
    protected void CloseAddToCollectionModal()
    {
        // Wait for dialog to appear
        wait.Until(d => d.FindElements(dialogLocator).Any());

        // Send global ESC to close
        new Actions(driver).SendKeys(Keys.Escape).Perform();

        // Wait for dialog to disappear
        wait.Until(d => d.FindElements(dialogLocator).Count == 0);
    }

}
