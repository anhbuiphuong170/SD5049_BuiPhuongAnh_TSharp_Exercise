using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Unsplash.Automation.Tests.Pages;

public abstract class BasePage
{
    protected IWebDriver driver;
    protected WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    protected IWebElement Visible(By locator)
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected void Click(By locator)
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);

        Thread.Sleep(500); 

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
    protected void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
    }

    protected void WaitPageStable()
    {
        Thread.Sleep(500); 
    }
    protected void RefreshAndWait()
        {
            driver.Navigate().Refresh();

            wait.Until(d =>
            {
                var readyState = ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState");
                return readyState != null && readyState.ToString() == "complete";
            });

            Thread.Sleep(800);
        }    
    protected void CloseAddToCollectionModal()
{
    // Đợi dialog hiển thị
    wait.Until(d => d.FindElements(By.XPath("//div[@role='dialog']")).Any());

    // Gửi phím ESC toàn cục (không cần focus vào button)
    new Actions(driver).SendKeys(Keys.Escape).Perform();

    // Đợi dialog biến mất
    wait.Until(d => d.FindElements(By.XPath("//div[@role='dialog']")).Count == 0);
}

}
