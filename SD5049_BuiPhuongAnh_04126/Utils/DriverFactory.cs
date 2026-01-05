namespace Unsplash.Automation.Tests.Utils;

public static class DriverFactory
{
    public static IWebDriver CreateChrome()
{
    var options = new ChromeOptions();

    options.AddArgument("--disable-notifications");
    options.AddArgument("--disable-infobars");
    options.AddArgument("--disable-extensions");

    var driver = new ChromeDriver(options);

    driver.Manage().Window.Maximize();

    return driver;
}

}
