namespace Unsplash.Automation.Tests.Pages;
public class CollectionDetailPage : BasePage
{
    public CollectionDetailPage(IWebDriver driver) : base(driver) { }

    private By photos = By.CssSelector("figure[itemprop='image']");

    public int CountPhotos()
    {
        RefreshAndWait();
        driver.Navigate().Refresh();
        wait.Until(d => d.FindElements(photos).Count >= 0);
        return driver.FindElements(photos).Count;
    }
}
