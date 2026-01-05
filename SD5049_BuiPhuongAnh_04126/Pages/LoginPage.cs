using Unsplash.Automation.Tests.Pages.Components;

namespace Unsplash.Automation.Tests.Pages;

public class LoginPage : BasePage
{
    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Login(string email, string password)
    {
        driver.Navigate().GoToUrl("https://unsplash.com/login");

        System.Threading.Thread.Sleep(5000); // wait for 5 seconds to load the login page completely
        var emailInput = Visible(By.CssSelector("input[type='email']"));
        emailInput.Clear();
        emailInput.SendKeys(email);

        var passwordInput = Visible(By.CssSelector("input[type='password']"));
        passwordInput.Clear();
        passwordInput.SendKeys(password);

        Click(By.XPath("//button[@type='submit' and contains(text(),'Login')]"));
        // Verify login via Header component
        new UserHeader(driver).WaitUntilLoggedIn();
    }
}
