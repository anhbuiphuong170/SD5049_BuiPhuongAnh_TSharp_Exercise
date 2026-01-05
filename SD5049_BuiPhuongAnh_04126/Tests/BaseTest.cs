using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Tests;

public abstract class BaseTest
{
    protected IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = DriverFactory.CreateChrome();
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}
