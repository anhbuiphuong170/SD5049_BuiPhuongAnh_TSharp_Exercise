namespace Unsplash.Automation.Tests.Pages;

public class HomePage : BasePage
{
    public HomePage(IWebDriver driver) : base(driver) { }
    // private By photos => By.CssSelector("figure[itemprop='image']");
    // private By bookmarkButtonInPhoto =>By.CssSelector("button[aria-label='Bookmark']");
    private By photoCards = By.CssSelector("figure[itemprop='image']");
    private By bookmarkButtons = By.CssSelector("button[aria-label='Bookmark']");
    private By addToCollectionBtn = By.XPath(".//button[@aria-label='Add to Collection']");
    private By createCollectionBtn = By.CssSelector("button[class*='createCollectionButton']");
    private By nameInput = By.CssSelector("input[name='title']");
    private By privateCheckbox = By.CssSelector("input[name='privacy']");
    private By createSubmitBtn = By.XPath("//button[.//span[text()='Create collection']]");
    private By collectionOptions = By.XPath("//div[@role='option']");
    private By collectionsList = By.XPath("//div[@role='listbox' or @aria-label='Collections']");
    public void Open()
    {
        driver.Navigate().GoToUrl("https://unsplash.com");
        wait.Until(d => d.Url.Contains("unsplash.com"));
        wait.Until(d => d.FindElements(photoCards).Count > 0);
    }

    public void OpenFirstPhoto()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

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
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript(
                            "arguments[0].scrollIntoView({block:'center'});",
                            photo
                        );

                    Thread.Sleep(300);

                    // Find bookmark button INSIDE photo
                    var bookmarkBtn = photo.FindElements(
                        By.CssSelector("button[aria-label='Bookmark']")
                    ).FirstOrDefault();

                    if (bookmarkBtn == null)
                        continue;

                    // JS click (DO NOT use Actions)
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].click();", bookmarkBtn);

                    bookmarked++;
                    Thread.Sleep(600);
                }
                catch (StaleElementReferenceException)
                {
                    continue;
                }
            }

            // Scroll to load more photos
            ((IJavaScriptExecutor)driver)
                .ExecuteScript("window.scrollBy(0, window.innerHeight);");

            Thread.Sleep(800);
        }

        if (bookmarked < count)
            throw new Exception($"Only bookmarked {bookmarked}/{count} photos");
    }

public string CreateCollectionAndAddPhotos(string collectionName, int count)
{
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    string collectionId = null;

    for (int i = 0; i < count; i++)
    {
        // 🔹 Scroll photo vào giữa màn hình
        ((IJavaScriptExecutor)driver).ExecuteScript(@"
            const photos = document.querySelectorAll(""figure[itemprop='image']"");
            photos[arguments[0]].scrollIntoView({ block: 'center' });
        ", i);

        Thread.Sleep(600);

        // 🔹 Click Add to Collection
        ((IJavaScriptExecutor)driver).ExecuteScript(@"
            const photos = document.querySelectorAll(""figure[itemprop='image']"");
            photos[arguments[0]]
                .querySelector(""button[aria-label='Add to Collection']"")
                .click();
        ", i);

        // 🔹 Wait popup open
        var dialog = wait.Until(d =>
            d.FindElements(By.XPath("//div[@role='dialog']")).FirstOrDefault()
        );

        if (i == 0)
        {
            // 🔹 Create new collection
            wait.Until(ExpectedConditions.ElementToBeClickable(createCollectionBtn)).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(nameInput)).SendKeys(collectionName);
            driver.FindElement(privateCheckbox).Click();
            driver.FindElement(createSubmitBtn).Click();

            // 🔹 Get collectionId
            collectionId = wait.Until(d =>
                d.Url.Contains("/collections/")
                    ? d.Url.Split("/collections/")[1].Split("/")[0]
                    : null
            );
        }
        else
        {
            // 🔹 Tick existing collection
            wait.Until(d =>
                d.FindElements(collectionOptions)
                 .Any(c => c.Text.Trim() == collectionName)
            );

            driver.FindElements(collectionOptions)
                  .First(c => c.Text.Trim() == collectionName)
                  .Click();
        }

        // ✅ BẮT BUỘC: ESC sau MỖI lần add
        CloseAddToCollectionModal();

        Thread.Sleep(800); // đảm bảo popup đóng hẳn trước vòng sau
    }

    return collectionId;
}

}