using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Unsplash.Automation.Tests.Utils;


namespace Unsplash.Automation.Tests.Pages;

public class PhotoDetailPage : BasePage
{
    private By photographerContainer => By.CssSelector(".photographer-Pgpa9y");
    private By viewProfileLink => By.XPath("//a[normalize-space()='View profile']");

    public PhotoDetailPage(IWebDriver driver) : base(driver) { }

    public void ViewPhotographerProfile()
    {
        // Hover the photographer area to reveal the 'View profile' link. We retry hover inside the wait
        // to mitigate flaky reveal behaviour on dynamically rendered pages.
        Hover(photographerContainer);

        var profileLink = wait.Until(d =>
        {
            try
            {
                var el = d.FindElements(viewProfileLink).FirstOrDefault();
                if (el != null && el.Displayed)
                    return el;

                // If link not visible yet, try hovering again to reveal it
                try { Hover(photographerContainer); } catch { }
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        });

        // Scroll and click; higher-level wait helpers in BasePage handle stability and JS fallback.
        ScrollIntoView(profileLink);
        TestConfig.Pause();
        profileLink.Click();

        // Verify navigated to profile page
        wait.Until(d => d.Url.Contains("/@"));
        TestConfig.Pause();
    }
    public void Download()
    {
        // Try multiple selectors
        // 1. Title "Download photo" (Main button usually)
        // 2. Link with text "Download free"
        // 3. Any link with download attribute as fallback
        
        try {
             var downloadBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@title='Download photo']")));
             downloadBtn.Click();
             Logger.Debug("Clicked Download by Title");
        }
        catch (WebDriverTimeoutException)
        {
             try {
                var btn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Download free']/ancestor::a")));
                btn.Click();
                Logger.Debug("Clicked Download by Text");
             }
             catch (WebDriverTimeoutException)
             {
                 // Fallback to "small" download button sometimes present?
                 // Or just log current page source snippet?
                 Logger.Debug("Failed to find download button. trying generic a[download]");
                 var btn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[download]")));
                 btn.Click();
             }
        }
    }
}
