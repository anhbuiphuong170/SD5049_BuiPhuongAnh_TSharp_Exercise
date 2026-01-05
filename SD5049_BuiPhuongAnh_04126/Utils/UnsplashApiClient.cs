namespace Unsplash.Automation.Tests.Utils;

public class UnsplashApiClient
{
    private readonly HttpClient client;

    public UnsplashApiClient(string cookies)
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("Cookie", cookies);
    }

    public async Task RemovePhoto(string collectionId, string photoId)
    {
        var req = new HttpRequestMessage(
            HttpMethod.Delete,
            $"https://unsplash.com/napi/collections/{collectionId}/photos/{photoId}"
        );

        var res = await client.SendAsync(req);
        res.EnsureSuccessStatusCode();
    }
}