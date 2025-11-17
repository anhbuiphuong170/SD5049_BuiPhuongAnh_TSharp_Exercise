using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using DemoQATests.Helpers;

namespace DemoQATests.Pages
{
    public class ProfilePage : BasePage
    {
        public ProfilePage(IWebDriver driver) : base(driver) { }

        private By searchBox => By.Id("searchBox");
        private By bookLinks => By.CssSelector("div.rt-tbody a");
        private By confirmDeleteBtn => By.Id("closeSmallModal-ok");

        public void SearchBook(string bookName)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(searchBox));

            var box = Find(searchBox);
            box.Clear();
            box.SendKeys(bookName);
        }

        public bool IsBookDisplayed(string bookName)
        {
            var tableBody = Find(By.CssSelector("div.rt-tbody"));
            if (tableBody.Text.Contains("No rows found", StringComparison.OrdinalIgnoreCase))
                return false;

            return FindAll(bookLinks).Any(b => b.Text == bookName);
        }

        public void DeleteBook(string bookName)
        {
            var book = FindAll(bookLinks).FirstOrDefault(b => b.Text == bookName);
            if (book == null) return;

            var row = book.FindElement(By.XPath("./ancestor::div[contains(@class,'rt-tr-group')]"));
            var deleteBtn = row.FindElement(By.CssSelector("span[title='Delete']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", deleteBtn);

            try { deleteBtn.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", deleteBtn); }

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var okBtn = wait.Until(ExpectedConditions.ElementToBeClickable(confirmDeleteBtn));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", okBtn);
            try { okBtn.Click(); }
            catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", okBtn); }

            AlertHelper.AcceptAlertIfPresent(driver);
        }

        public void RefreshProfile()
        {
            driver.Navigate().Refresh();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(searchBox));
        }
    }
}