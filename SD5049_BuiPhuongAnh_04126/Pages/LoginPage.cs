using Unsplash.Automation.Tests.Pages.Components;

namespace Unsplash.Automation.Tests.Pages;

public class LoginPage : BasePage
{
    private By emailSelector = By.CssSelector("input[type='email']");
    private By passwordSelector = By.CssSelector("input[type='password']");
    private By submitButton = By.XPath("//button[@type='submit' and contains(text(),'Login')]");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Login(string email, string password)
    {
        driver.Navigate().GoToUrl("https://unsplash.com/login");

        var emailInput = Visible(emailSelector);
        emailInput.Clear();
        emailInput.SendKeys(email);

        var passwordInput = Visible(passwordSelector);
        passwordInput.Clear();
        passwordInput.SendKeys(password);

        Click(submitButton);
        // Verify login via Header component
        new UserHeader(driver).WaitUntilLoggedIn();
    }
}
