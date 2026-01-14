using Unsplash.Automation.Tests.Utils;

namespace Unsplash.Automation.Tests.Pages;
public class CollectionDetailPage : BasePage
{
    private By photos = By.CssSelector("figure[itemprop='image']");
    private By addToCollectionButton = By.CssSelector("button[aria-label='Add to Collection']");

    public CollectionDetailPage(IWebDriver driver) : base(driver) { }

    /// <summary>
    /// Return the number of photos visible in the collection detail.
    /// Uses RefreshAndWait() to ensure the page is stable before counting.
    /// </summary>
    public int CountPhotos()
    {
        RefreshAndWait();
        return driver.FindElements(photos).Count;
    }

    /// <summary>
    /// Attempts to remove a photo from the named collection by opening the photo's
    /// collection modal and toggling the collection option. Returns true on success.
    /// The method retries a small number of photos and will throw if none were removed.
    /// </summary>
    public bool RemoveAPhotoFromCollection(string collectionName)
    {
        var photoElements = wait.Until(d => d.FindElements(photos));
        if (photoElements.Count == 0) throw new Exception("No photos found in collection");

        bool removed = false;

        for (int i = 0; i < Math.Min(photoElements.Count, 5); i++)
        {
            // Refetch to avoid StaleElementReferenceException if DOM updated
            var currentPhotos = driver.FindElements(photos);
            if (i >= currentPhotos.Count) break;
            var photo = currentPhotos[i];
            
            // Hover to reveal controls
            Hover(photo);
            WaitPageStable();

            // Click "Add to Collection" button
            try {
                    var manageBtn = photo.FindElement(addToCollectionButton);
                manageBtn.Click();
            } catch {
                Logger.Debug($"Could not click manage button for photo {i}");
                continue;
            }

                // Wait for modal
                try {
                var modal = wait.Until(d => d.FindElement(dialogLocator));

                // Find option matching our collection
                var collectionOption = wait.Until(d =>
                    d.FindElements(optionLocator)
                     .FirstOrDefault(c => c.Text.Contains(collectionName))
                );

                if (collectionOption != null)
                {
                     // Check if ticked (SVG presence)
                     bool isChecked = collectionOption.FindElements(By.TagName("svg")).Count > 0;
                     Logger.Debug($"Photo {i} in collection '{collectionName}'? {isChecked}");
                     
                     if (isChecked)
                     {
                         collectionOption.Click();
                         Logger.Debug($"Removed photo {i} from collection.");
                         removed = true;
                         // Close modal
                         CloseAddToCollectionModal();
                         break; // Done
                     }
                     else
                     {
                         Logger.Debug($"Photo {i} is NOT in collection (likely related photo). Skipping.");
                         // Close modal and continue
                         CloseAddToCollectionModal();
                     }
                }
                else
                {
                     Logger.Debug($"Collection {collectionName} not found in modal for photo {i}");
                     CloseAddToCollectionModal();
                }
            }
            catch (Exception ex)
            {
                Logger.Debug($"Error processing photo {i}: {ex.Message}");
                try { CloseAddToCollectionModal(); } catch {}
            }
        }
        
        if (!removed) throw new Exception($"Failed to remove any photo from collection {collectionName}");

        return removed;
    }

    /// <summary>
    /// Return the collection id parsed from the current URL (segments: /collections/{id}/...)
    /// </summary>
    public string GetCurrentCollectionId()
    {
        var uri = new Uri(driver.Url);
        return uri.Segments[2].TrimEnd('/');
    }
}
