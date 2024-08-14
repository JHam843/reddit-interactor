namespace RedditStatsTracker.Models
{
    public class RedditConfig
    {
        public string ClientId { get; set; }           // Reddit API Client ID
        public string ClientSecret { get; set; }       // Reddit API Client Secret
        public string UserAgent { get; set; }          // User-Agent string for the Reddit API
        public string Subreddit { get; set; }          // Subreddit to track
        public int RateLimitBuffer { get; set; }       // Buffer for rate limiting (number of requests before rate limit)
    }
}