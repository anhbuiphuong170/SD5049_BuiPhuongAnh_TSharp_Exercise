using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Unsplash.Automation.Tests.Utils;

public class UnsplashApiClient
{
    private readonly HttpClient client;

    // UnsplashApiClient: lightweight API helper used for test data setup/teardown.
    // It is intentionally small and uses direct HTTP calls with cookies from the browser session.

    public UnsplashApiClient(string cookies, string? csrfToken = null)
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("Cookie", cookies);
        if (!string.IsNullOrEmpty(csrfToken))
        {
            client.DefaultRequestHeaders.Add("X-CSRF-Token", csrfToken);
        }
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
    }

    public async Task<string> CreateCollection(string title, bool isPrivate)
    {
        var payload = new { title, @private = isPrivate };
        var req = new HttpRequestMessage(HttpMethod.Post, "https://unsplash.com/napi/collections")
        {
            Content = JsonContent.Create(payload)
        };

        var res = await client.SendAsync(req);
        if (!res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            throw new Exception($"API Error {res.StatusCode}: {content}");
        }
        res.EnsureSuccessStatusCode();
        
        var json = await res.Content.ReadFromJsonAsync<JsonNode>();
        return json?["id"]?.ToString() ?? throw new Exception("Failed to get collection ID");
    }

    public async Task AddPhotoToCollection(string collectionId, string photoId)
    {
        var payload = new { photo_id = photoId };
        var req = new HttpRequestMessage(HttpMethod.Post, $"https://unsplash.com/napi/collections/{collectionId}/add")
        {
            Content = JsonContent.Create(payload)
        };

        var res = await client.SendAsync(req);
        res.EnsureSuccessStatusCode();
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

    public async Task DeleteCollection(string collectionId)
    {
        var req = new HttpRequestMessage(
            HttpMethod.Delete,
            $"https://unsplash.com/napi/collections/{collectionId}"
        );

        var res = await client.SendAsync(req);
        res.EnsureSuccessStatusCode();
    }

     public async Task<string> GetRandomPhotoId()
    {
        // Using public API or NAPI? 
        // NAPI: https://unsplash.com/napi/photos/random?count=1
        var req = new HttpRequestMessage(HttpMethod.Get, "https://unsplash.com/napi/photos/random?count=1");
        var res = await client.SendAsync(req);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadFromJsonAsync<JsonNode>();
        return json?[0]?["id"]?.ToString() ?? throw new Exception("Failed to get random photo ID");
    }

    public async Task<List<string>> GetRandomPhotoIds(int count)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"https://unsplash.com/napi/photos/random?count={count}");
        var res = await client.SendAsync(req);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadFromJsonAsync<JsonArray>();
        return json?.Select(x => x?["id"]?.ToString() ?? "").ToList() ?? new List<string>();
    }
}