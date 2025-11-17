using OpenQA.Selenium;
namespace DemoQATests.Pages
{
    /// <summary>
    /// Base class for all Page Object classes.
    /// Provides common Selenium helpers such as navigation and element lookup.
    /// </summary>
    public class BasePage
    {
        protected readonly IWebDriver driver;

        /// <summary>
        /// Constructor receives the WebDriver instance.
        /// </summary>
        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Navigate to a specified URL.
        /// </summary>
        public void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Find a single element using a locator.
        /// Throws exception if not found.
        /// </summary>
        public IWebElement Find(By by) => driver.FindElement(by);

        /// <summary>
        /// Find all matching elements.
        /// Returns empty collection if none found.
        /// </summary>
        public IReadOnlyCollection<IWebElement> FindAll(By by) => driver.FindElements(by);
    }
}
