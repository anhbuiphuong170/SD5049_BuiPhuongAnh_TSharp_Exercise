namespace Unsplash.Automation.Tests.Pages.Components;

public class UserHeader : BasePage
{
    public UserHeader(IWebDriver driver) : base(driver) { }

    public By AvatarIcon => By.CssSelector("img[alt^='Avatar of user']");

    public By ViewProfile => By.XPath("//span[normalize-space()='View profile']");

    public void OpenUserMenu()
    {
        Click(AvatarIcon);
    }

    public void GoToMyProfile()
    {
        OpenUserMenu();
        Click(ViewProfile);
        wait.Until(d => d.Url.Contains("/@"));
    }

    public void WaitUntilLoggedIn()
    {
        Visible(AvatarIcon);
    }
}
