using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Pages;

public class HomePage : BasePage
{
    // HomePage: page object representing Unsplash home view.
    // Responsibilities:
    // - Navigate to the home page, locate photo cards, and provide higher-level actions
    //   such as opening the first photo or bookmarking photos.
    // - Methods return primitive types or simple identifiers (e.g., collection id) so tests can
    //   assert outcomes without parsing the DOM directly.
    public HomePage(IWebDriver driver) : base(driver) { }
    private By photoCards = By.CssSelector("figure[itemprop='image']");
    private By bookmarkButtons = By.CssSelector("button[aria-label='Bookmark']");
    private By addToCollectionBtn = By.XPath(".//button[@aria-label='Add to Collection']");
    private By createCollectionBtn = By.CssSelector("button[class*='createCollectionButton']");
    private By nameInput = By.CssSelector("input[name='title']");
    private By privateCheckbox = By.CssSelector("input[name='privacy']");
    private By createSubmitBtn = By.XPath("//button[.//span[text()='Create collection']]");
    private By collectionOptions = By.XPath("//div[@role='option']");
    private By collectionsList = By.XPath("//div[@role='listbox' or @aria-label='Collections']");
    private By collectionSearchInput = By.CssSelector("input[placeholder*='Search']");
    
    public void Open()
    {
        driver.Navigate().GoToUrl("https://unsplash.com");
        wait.Until(d => d.Url.Contains("unsplash.com"));
        wait.Until(d => d.FindElements(photoCards).Count > 0);
    }
    /// <summary>
    /// Open the first photo in the grid by navigating directly to its href.
    /// Uses JavaScript to extract the link to avoid brittle DOM traversal.
    /// </summary>
    public void OpenFirstPhoto()
    {
        string? href = wait.Until(d =>
        {
            try
            {
                return (string?)((IJavaScriptExecutor)d).ExecuteScript(@"
                    const link = document.querySelector(""figure[itemprop='image'] a[href*='/photos/']"");
                    return link ? link.href : null;
                ");
            }
            catch
            {
                return null;
            }
        });

        if (string.IsNullOrEmpty(href))
            throw new Exception("Unable to retrieve the link to the first photo.");

        driver.Navigate().GoToUrl(href);

        wait.Until(d =>
        {
            if (d is not IJavaScriptExecutor js)
                return false;

            return js.ExecuteScript("return document.readyState")?.ToString() == "complete";
        });
    }

    public void BookmarkRandomPhotos(int count)
    {
        int bookmarked = 0;
        int safety = 0;

        while (bookmarked < count && safety < 50)
        {
            safety++;

            var photos = wait.Until(d => d.FindElements(photoCards));

            foreach (var photo in photos)
            {
                if (bookmarked >= count)
                    return;

                try
                {
                    // Scroll photo to center of screen
                    ScrollIntoView(photo);

                    // Wait for bookmark button to appear inside the photo (re-hover if needed)
                    try
                    {
                        var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        shortWait.Until(d =>
                        {
                            try
                            {
                                var btn = photo.FindElements(bookmarkButtons).FirstOrDefault();
                                if (btn == null || !btn.Displayed || !btn.Enabled)
                                {
                                    Hover(photo);
                                    return false;
                                }
                                return true;
                            }
                            catch { return false; }
                        });
                    }
                    catch (WebDriverTimeoutException)
                    {
                        // If still not found, skip this photo
                        continue;
                    }

                    // Find bookmark button INSIDE photo
                    var bookmarkBtn = photo.FindElements(
                        bookmarkButtons
                    ).FirstOrDefault();

                    if (bookmarkBtn == null)
                        continue;

                    // JS click (DO NOT use Actions)
                        JsClick(bookmarkBtn);

                    bookmarked++;
                    // small stabilization wait for UI update
                    WaitPageStable();
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
            }

            // Scroll to load more photos and wait for new content
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, window.innerHeight);");
            wait.Until(d => d.FindElements(photoCards).Count > 0);
        }

        if (bookmarked < count)
            throw new Exception($"Only bookmarked {bookmarked}/{count} photos");
    }
public (string collectionId, CollectionDetailPage page) CreateCollectionAndAddPhotos(string collectionName, int count)
{
    string? collectionId = null;

    for (int i = 0; i < count; i++)
    {
        var targetPhoto = GetPhotoAtIndex(i);
        if (targetPhoto == null)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, window.innerHeight);");
            TestConfig.Pause();
            targetPhoto = GetPhotoAtIndex(i);
            if (targetPhoto == null) throw new Exception($"Not enough photos to add: requested index {i}");
        }

            ScrollIntoView(targetPhoto);
        TestConfig.Pause();

        IWebElement addBtn;
        try
        {
            addBtn = targetPhoto.FindElement(addToCollectionBtn);
        }
        catch (StaleElementReferenceException)
        {
            targetPhoto = GetPhotoAtIndex(i) ?? throw new Exception($"Photo at index {i} became unavailable");
            addBtn = targetPhoto.FindElement(addToCollectionBtn);
        }

        JsClick(addBtn);

        var dialog = wait.Until(d => d.FindElements(dialogLocator).FirstOrDefault());

        if (i == 0)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(createCollectionBtn)).Click();
            var input = wait.Until(ExpectedConditions.ElementIsVisible(nameInput));
            input.Click();
            input.SendKeys(collectionName);
            driver.FindElement(privateCheckbox).Click();
            var btn = driver.FindElement(createSubmitBtn);
            btn.Click();
            TestConfig.Pause();
        }
        else
        {
            bool found = false;
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    try { wait.Until(d => d.FindElements(collectionOptions).Count > 0); } catch {}
                    var search = driver.FindElements(collectionSearchInput).FirstOrDefault();
                    if (search != null && search.Displayed)
                    {
                        search.Clear();
                        search.SendKeys(collectionName);
                        TestConfig.Pause();
                    }

                    wait.Until(d => d.FindElements(collectionOptions).Any(c => c.Text.Contains(collectionName)));
                    driver.FindElements(collectionOptions).First(c => c.Text.Contains(collectionName)).Click();
                    found = true;
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Debug($"Attempt {attempt+1} failed: {ex.Message}");
                    CloseAddToCollectionModal();
                    TestConfig.Pause();

                    try
                    {
                        var p = GetPhotoAtIndex(i);
                        if (p != null)
                        {
                            var ab = p.FindElement(addToCollectionBtn);
                            JsClick(ab);
                            TestConfig.Pause();
                        }
                    }
                    catch (StaleElementReferenceException) { }
                }
            }

            if (!found) throw new Exception($"Failed to find collection {collectionName} after 3 attempts");
        }

        CloseAddToCollectionModal();
        TestConfig.Pause();
    }

    // After adding photos, navigate to Collections and open the created collection to retrieve its id
    var collectionsPage = new CollectionsPage(driver);
    collectionsPage.OpenCollections();
    var collectionPage = collectionsPage.OpenCollection(collectionName);
    collectionId = collectionPage.GetCurrentCollectionId();

    return (collectionId ?? string.Empty, collectionPage);
}

    // Private helpers
    // Keep helper methods after public actions for consistent ordering
    private IWebElement GetPhotoAtIndex(int index)
    {
        return wait.Until(d =>
        {
            var list = d.FindElements(photoCards);
            return (list.Count > index) ? list[index] : null;
        });
    }

}