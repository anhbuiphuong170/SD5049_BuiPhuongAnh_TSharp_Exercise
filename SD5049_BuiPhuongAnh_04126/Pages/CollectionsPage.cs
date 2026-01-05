namespace Unsplash.Automation.Tests.Pages;

public class CollectionsPage : BasePage
{
    public CollectionsPage(IWebDriver driver) : base(driver) { }

    private By collectionsTab = By.CssSelector("a[aria-label='Collections']");
    private By collectionPhotos = By.CssSelector("figure[itemprop='image']");

    public void OpenCollections()
    {
        var tab = wait.Until(d =>
            d.FindElements(collectionsTab).First(e => e.Displayed)
        );
        tab.Click();
        wait.Until(d => d.Url.Contains("/collections"));
    }

    public void OpenCollection(string name)
    {
        var card = wait.Until(d =>
            d.FindElement(By.XPath($"//div[text()=\"{name}\"]"))
        );
        card.Click();
        wait.Until(d => d.Url.Contains("/collections/"));
    }

    public int CountPhotos()
    {
        driver.Navigate().Refresh();
        wait.Until(d => d.FindElements(collectionPhotos).Count >= 0);
        return driver.FindElements(collectionPhotos).Count;
    }

    public string GetFirstPhotoId()
    {
        var href = driver.FindElement(collectionPhotos)
            .FindElement(By.CssSelector("a[href*='/photos/']"))
            .GetAttribute("href");

        return href?.Split("/photos/")[1] ?? string.Empty;
    }
    public string GetCurrentCollectionId()
{
    var uri = new Uri(driver.Url);
    // /collections/{id}/{slug}
    return uri.Segments[2].TrimEnd('/');
}

}


