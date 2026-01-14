using System;
using System.Linq;
using OpenQA.Selenium;

namespace Unsplash.Automation.Tests.Utils;

public static class ApiClientFactory
{
    // ApiClientFactory: helper to create API clients from WebDriver state.
    // This centralizes cookie/CSRF extraction so tests do not repeat driver plumbing.
    public static UnsplashApiClient CreateFromDriver(IWebDriver driver)
    {
        var cookies = driver.Manage().Cookies.AllCookies;
        var cookieString = string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));

        string? csrfToken = null;
        try
        {
            csrfToken = (string?)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('meta[name=\"csrf-token\"]')?.content;");
        }
        catch { }

        return new UnsplashApiClient(cookieString, csrfToken);
    }
}
