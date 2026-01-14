using System;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Unsplash.Automation.Tests.Utils;

public static class CleanupHelper
{
    // CleanupHelper: centralizes teardown actions used by tests (e.g. deleting collections).
    // Attempts API delete first; if it fails and a driver is provided, falls back to UI deletion.
    public static async Task DeleteCollectionAsync(UnsplashApiClient apiClient, string collectionId, IWebDriver? driver = null)
    {
        try
        {
            await apiClient.DeleteCollection(collectionId);
            Logger.Info($"Deleted collection {collectionId} via API");
            return;
        }
        catch (Exception ex)
        {
            Logger.Warn($"API delete failed for collection {collectionId}: {ex.Message}");
        }

        if (driver == null)
        {
            Logger.Warn($"No driver available to attempt UI deletion for collection {collectionId}");
            return;
        }

        try
        {
            // Navigate to the collection page and try to delete via UI
            var collectionUrl = $"https://unsplash.com/collections/{collectionId}";
            driver.Navigate().GoToUrl(collectionUrl);

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains($"/collections/{collectionId}"));

            // Ensure logged in; if not, perform login
            try
            {
                var header = new Pages.Components.UserHeader(driver);
                header.WaitUntilLoggedIn();
            }
            catch
            {
                try
                {
                    new Pages.LoginPage(driver).Login(TestConfig.Email, TestConfig.Password);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Unable to login during UI cleanup: {ex.Message}");
                }
            }

            // Try several selectors that might represent a delete action
            string[] deleteXPaths = new[] {
                "//button[contains(., 'Delete collection')]",
                "//button[contains(., 'Delete') and contains(@class,'collection')]",
                "//button[contains(., 'Delete')]",
                "//a[contains(., 'Delete collection')]",
                "//button[@aria-label='Delete']"
            };

            bool deleted = false;

            foreach (var xp in deleteXPaths)
            {
                try
                {
                    var btn = wait.Until(d => d.FindElements(By.XPath(xp)).FirstOrDefault(e => e.Displayed));
                    if (btn != null)
                    {
                        try { btn.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btn); }

                        // Look for a confirmation button in any modal
                        try
                        {
                            var confirm = wait.Until(d => d.FindElements(By.XPath("//button[contains(., 'Delete') and (contains(@class,'danger') or contains(@class,'confirm') or contains(., 'Delete'))]")).FirstOrDefault(e => e.Displayed));
                            if (confirm != null)
                            {
                                try { confirm.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", confirm); }
                            }
                        }
                        catch { }

                        // Small wait and verify
                        System.Threading.Thread.Sleep(1000);
                        if (!driver.Url.Contains($"/collections/{collectionId}"))
                        {
                            deleted = true;
                            Logger.Info($"Deleted collection {collectionId} via UI (selector {xp})");
                            break;
                        }
                    }
                }
                catch (WebDriverTimeoutException) { }
                catch (Exception ex) { Logger.Debug($"UI delete attempt failed for xpath {xp}: {ex.Message}"); }
            }

            if (!deleted)
                Logger.Warn($"UI delete did not detect successful navigation away from collection {collectionId}");
        }
        catch (Exception ex)
        {
            Logger.Warn($"Failed to delete collection {collectionId} via UI fallback: {ex.Message}");
        }
    }
}
