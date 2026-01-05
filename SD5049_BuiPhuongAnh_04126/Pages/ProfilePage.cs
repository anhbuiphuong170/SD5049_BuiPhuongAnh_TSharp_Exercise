namespace Unsplash.Automation.Tests.Pages;

public class ProfilePage : BasePage
{
    public ProfilePage(IWebDriver driver) : base(driver) { }

    private By editProfileLink =>
        By.XPath("//a[contains(text(),'Edit profile')]");

    private By usernameInput =>
        By.Id("user_username");

    private By updateAccountBtn =>
        By.CssSelector("input[type='submit'][value='Update account']");

    private By fullNameLabel =>
        By.CssSelector("div.name-FdAJI1.responsiveHeadingL-_kGdqo");

    public string UpdateUsername(string baseUsername)
    {
        Click(editProfileLink);

        wait.Until(d => d.FindElement(usernameInput).Displayed);

        var newUsername = $"{baseUsername}{DateTime.Now:yyyyMMddHHmmss}";

        var input = driver.FindElement(usernameInput);
        input.Clear();
        input.SendKeys(newUsername);

        Click(updateAccountBtn);

        return newUsername;
    }

    public void OpenProfileByUsername(string username)
    {
        driver.Navigate().GoToUrl($"https://unsplash.com/@{username}");
        wait.Until(d => d.Url.Contains(username));
        Thread.Sleep(2000);
    }

    public string GetFullName()
    {
        return wait.Until(d =>
            d.FindElement(fullNameLabel)).Text;
    }
}
