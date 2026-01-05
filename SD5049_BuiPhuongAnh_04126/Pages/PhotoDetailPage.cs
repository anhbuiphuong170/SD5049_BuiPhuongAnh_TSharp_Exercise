using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Unsplash.Automation.Tests.Pages;

public class PhotoDetailPage : BasePage
{
    public PhotoDetailPage(IWebDriver driver) : base(driver) { }

    private By photographerContainer => By.CssSelector(".photographer-Pgpa9y");
    private By viewProfileLink => By.XPath("//a[normalize-space()='View profile']");

    public void ViewPhotographerProfile()
    {
        Hover(photographerContainer);

        var profileLink = wait.Until(d =>
        {
            var el = d.FindElements(viewProfileLink).FirstOrDefault();
            return (el != null && el.Displayed) ? el : null;
        });

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", profileLink);

        Thread.Sleep(2000);
        profileLink.Click();

        // ✅ Verify đã vào profile
        wait.Until(d => d.Url.Contains("/@"));
        Thread.Sleep(2000);
    }
    public void Download()
        {
            Click(By.CssSelector("a[download]"));
        }
}
