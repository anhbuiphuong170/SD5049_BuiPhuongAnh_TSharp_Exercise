using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoQATests.Services
{
    public class ApiService
    {
        private readonly RestClient client;

        public ApiService()
        {
            client = new RestClient(new RestClientOptions("https://demoqa.com") { Timeout = TimeSpan.FromSeconds(10) });
        }

        private async Task<RestResponse> SendAsync(string endpoint, Method method, object? body = null, string? token = null)
        {
            var request = new RestRequest(endpoint, method);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            if (body != null)
                request.AddStringBody(JsonConvert.SerializeObject(body), DataFormat.Json);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"API Error: {method} {endpoint} → {response.StatusCode}");
                Console.WriteLine($"Content: {response.Content}");
            }

            return response;
        }

        public async Task<(string UserId, string Token)> LoginAsync(string username, string password)
        {
            var body = new { userName = username, password = password };
            var response = await SendAsync("/Account/v1/Login", Method.Post, body);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                throw new Exception("Login failed");

            dynamic json = JsonConvert.DeserializeObject(response.Content)!;
            return (json.userId, json.token);
        }

        public async Task<List<string>> GetUserBookIsbnsAsync(string userId, string token)
        {
            var response = await SendAsync($"/Account/v1/User/{userId}", Method.Get, null, token);
            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content)) return new();

            var data = JsonConvert.DeserializeObject<UserBooksResponse>(response.Content);
            return data?.Books?.Where(b => b.Isbn != null).Select(b => b.Isbn!).ToList() ?? new();
        }

        public async Task AddBookAsync(string userId, string isbn, string token)
        {
            var body = new { userId, collectionOfIsbns = new[] { new { isbn } } };
            var response = await SendAsync("/BookStore/v1/Books", Method.Post, body, token);

            if (!response.IsSuccessful)
                throw new Exception($"AddBook failed: {response.StatusCode}");
        }

        public async Task DeleteBookAsync(string userId, string isbn, string token)
        {
            var body = new { isbn, userId };
            var response = await SendAsync("/BookStore/v1/Book", Method.Delete, body, token);

            if (!response.IsSuccessful)
                throw new Exception($"DeleteBook failed: {response.StatusCode}");
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var response = await SendAsync("/BookStore/v1/Books", Method.Get);
            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                throw new Exception("Failed to fetch books");

            dynamic json = JsonConvert.DeserializeObject(response.Content)!;
            var books = new List<Book>();
            foreach (var b in json.books)
                books.Add(new Book { Isbn = b.isbn, Title = b.title });

            return books;
        }

        public async Task AddAllBooksAsync(string userId, string token)
        {
            var allBooks = await GetAllBooksAsync();
            var ownedIsbns = await GetUserBookIsbnsAsync(userId, token);

            foreach (var book in allBooks)
            {
                if (!string.IsNullOrEmpty(book.Isbn) && !ownedIsbns.Contains(book.Isbn))
                    await AddBookAsync(userId, book.Isbn!, token);
            }
        }
    }

    public class UserBooksResponse
    {
        public string? UserId { get; set; }
        public Book[]? Books { get; set; }
    }

    public class Book
    {
        public string? Isbn { get; set; }
        public string? Title { get; set; }
    }
}