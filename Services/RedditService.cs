using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using RedditStatsTracker.Interfaces;
using RedditStatsTracker.Models;

namespace RedditStatsTracker.Services
{
    public class RedditService : IRedditService
    {
        private readonly HttpClient _httpClient;
        private readonly RedditConfig _config;

        // Constructor that initializes HttpClient and RedditConfig
        public RedditService(HttpClient httpClient, RedditConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Method to fetch recent posts from the specified subreddit
        public async Task<IEnumerable<Post>> GetRecentPostsAsync(string subreddit)
        {
            // Ensure authentication
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_config.UserAgent);

            // Send a GET request to fetch the latest posts
            var response = await _httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/new.json");
            response.EnsureSuccessStatusCode(); // Throw an exception if the request was not successful

            // Deserialize the JSON response into a RedditResponse object
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<RedditResponse>(content);

            // Return the list of posts
            return posts?.Data.Children.Select(c => c.Data) ?? Enumerable.Empty<Post>();
        }

        // Method to obtain an OAuth2 access token
        private async Task<string> GetAccessTokenAsync()
        {
            // Set the authorization header for basic authentication (using client ID and secret)
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config.ClientId}:{_config.ClientSecret}"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
            // _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.29.2");
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("RedditStatsTracker/1.0 (by /u/Fun-Hand-6218)");

            // Prepare the form data for the token request
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "read")
            });

            // Send a POST request to obtain the access token
            var response = await _httpClient.PostAsync("https://www.reddit.com/api/v1/access_token", requestBody);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Deserialize and return the access token
            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            // Return the access token
            return tokenResponse.AccessToken;
        }

    }

    // Class to represent the structure of the token response from Reddit
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
