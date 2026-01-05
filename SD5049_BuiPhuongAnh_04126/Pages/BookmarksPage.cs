using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Unsplash.Automation.Tests.Pages
{
    public class BookmarksPage : BasePage
    {
        public BookmarksPage(IWebDriver driver) : base(driver) { }

        private By clearButton => By.CssSelector("button.clearAllBookmarksButton-i3dfHy");
        private By confirmClearButton => By.XPath("//button[normalize-space()='Clear bookmarks']");
        private By bookmarkedPhotos => By.CssSelector("figure[itemprop='image']");
        private By bookmarksLinks => By.CssSelector("a[href='/bookmarks'][aria-label='Bookmarks']");

        public void Open()
        {
            driver.Navigate().GoToUrl("https://unsplash.com/bookmarks");
            wait.Until(d => d.Url.Contains("/bookmarks"));
        }
        public void ClickBookmarks()
        {
            // Get all elements with the Bookmarks selector
            var bookmarkLinks = driver.FindElements(bookmarksLinks);

            // Select the first element that is displayed and has valid size
            var visibleLink = bookmarkLinks.FirstOrDefault(el =>
                el.Displayed && el.Size.Height > 0 && el.Size.Width > 0);

            if (visibleLink != null)
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(visibleLink)).Click();
            }
            else
            {
                throw new Exception("Cannot find a clickable Bookmarks icon.");
            }
            Thread.Sleep(5000);

        }                     
        public int CountBookmarkedPhotos()
        {
            RefreshAndWait();
            return driver.FindElements(bookmarkedPhotos).Count;
        }
        public void ClearAllBookmarks()
        {
            if (CountBookmarkedPhotos() == 0)
                return;

            var clearBtn = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(clearButton)
            );
            clearBtn.Click();

            WaitPageStable();

            var confirmBtn = wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(confirmClearButton)
            );

            ((IJavaScriptExecutor)driver)
                .ExecuteScript("arguments[0].click();", confirmBtn);

            wait.Until(d => CountBookmarkedPhotos() == 0);
        }

    }
}
