using System;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Pages;

public class ProfilePage : BasePage
{
    private By editProfileLink => By.XPath("//a[contains(text(),'Edit profile')]");
    private By usernameInput => By.Id("user_username");
    private By updateAccountBtn => By.CssSelector("input[type='submit'][value='Update account']");
    private By fullNameLabel => By.CssSelector("div.name-FdAJI1.responsiveHeadingL-_kGdqo");
    public ProfilePage(IWebDriver driver) : base(driver) { }

    // ProfilePage: actions related to the user's profile page.
    // - UpdateUsername performs edit and save actions and returns the new username string.
    // - OpenProfileByUsername navigates directly to the public profile URL and waits for a stable
    //   indicator that the profile has loaded.

    public string UpdateUsername(string baseUsername)
    {
        Click(editProfileLink);

        wait.Until(d => d.FindElement(usernameInput).Displayed);

        var newUsername = $"{baseUsername}{DateTime.Now.Ticks.ToString()[^6..]}";

        var input = driver.FindElement(usernameInput);
        input.Clear();
        input.SendKeys(newUsername);

        Click(updateAccountBtn);

        return newUsername;
    }
    public void OpenProfileByUsername(string username)
    {
        driver.Navigate().GoToUrl($"https://unsplash.com/@{username}");

        // Wait until the document is ready
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.ToString() == "complete");

        // Wait for a reliable profile indicator: full name label, any H1, or og:title meta containing the username
        wait.Until(d =>
        {
            try
            {
                if (d.FindElements(fullNameLabel).Count > 0) return true;
                if (d.FindElements(By.TagName("h1")).Count > 0) return true;

                var ogObj = ((IJavaScriptExecutor)d).ExecuteScript("var m=document.querySelector('meta[property=\\\"og:title\\\"]'); return m?m.getAttribute('content'):null;");
                var og = ogObj as string;
                return !string.IsNullOrEmpty(og) && og.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch { return false; }
        });
    }
    public string GetFullName()
    {
        // Try robust strategies in order: h1, fullNameLabel, og:title parsing
        try
        {
            var h1 = wait.Until(d => d.FindElements(By.TagName("h1")).FirstOrDefault());
            var h1Text = h1?.Text;
            if (!string.IsNullOrWhiteSpace(h1Text))
                return h1Text.Trim();
        }
        catch { }

        try
        {
            var el = wait.Until(d => d.FindElements(fullNameLabel).FirstOrDefault());
            var elText = el?.Text;
            if (!string.IsNullOrWhiteSpace(elText))
                return elText.Trim();
        }
        catch { }

        try
        {
            var ogObj = ((IJavaScriptExecutor)driver).ExecuteScript("var m=document.querySelector('meta[property=\\\"og:title\\\"]'); return m?m.getAttribute('content'):null;");
            var ogStr = ogObj as string;
            if (!string.IsNullOrWhiteSpace(ogStr))
            {
                var s = ogStr;
                // Parse common separators like '·', ' on ', '(', '-' etc.
                var separators = new[] {"·", " on ", "(", " - ", " — ", "|"};
                int idx = -1;
                foreach (var sep in separators)
                {
                    idx = s.IndexOf(sep, StringComparison.Ordinal);
                    if (idx >= 0) break;
                }
                var name = (idx >= 0) ? s.Substring(0, idx) : s;
                name = name.Trim();
                // If name contains an '@' or username, remove trailing username
                if (name.Contains("@"))
                {
                    var parts = name.Split('@');
                    name = parts[0].Trim();
                }
                return name;
            }
        }
        catch { }

        return string.Empty;
    }

    // (Download helpers moved to Utils/DownloadHelper.cs)
}
