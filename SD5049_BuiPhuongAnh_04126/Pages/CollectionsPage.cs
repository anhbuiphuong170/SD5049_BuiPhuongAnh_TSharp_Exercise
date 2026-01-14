namespace Unsplash.Automation.Tests.Pages;

public class CollectionsPage : BasePage
{
    // CollectionsPage: operations to navigate collections and open specific collection cards.
    // Keep DOM interactions encapsulated here so tests don't rely on selectors directly.
    public CollectionsPage(IWebDriver driver) : base(driver) { }

    private By collectionsTab = By.CssSelector("a[aria-label='Collections']");
    private By collectionPhotos = By.CssSelector("figure[itemprop='image']");
    private By photoLink = By.CssSelector("a[href*='/photos/']");
    public void OpenCollections()
    {
        var tab = wait.Until(d =>
            d.FindElements(collectionsTab).First(e => e.Displayed)
        );
        tab.Click();
        wait.Until(d => d.Url.Contains("/collections"));
    }

    /// <summary>
    /// Open a collection by visible name and return the resulting CollectionDetailPage.
    /// </summary>
    public CollectionDetailPage OpenCollection(string name)
    {
        var card = wait.Until(d => d.FindElement(CardLocator(name)));
        card.Click();
        wait.Until(d => d.Url.Contains("/collections/"));
        return new CollectionDetailPage(driver);
    }

    private By CardLocator(string name) => By.XPath($"//div[text()=\"{name}\"]");

    public int CountPhotos()
    {
        RefreshAndWait();
        return driver.FindElements(collectionPhotos).Count;
    }

    public string GetFirstPhotoId()
    {
        var href = driver.FindElement(collectionPhotos).FindElement(photoLink).GetAttribute("href");

        return href?.Split("/photos/")[1] ?? string.Empty;
    }
    public string GetCurrentCollectionId()
{
    var uri = new Uri(driver.Url);
    // /collections/{id}/{slug}
    return uri.Segments[2].TrimEnd('/');
}

}


