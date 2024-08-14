namespace RedditStatsTracker.Utils
{
    public class RateLimiter
    {
        private readonly int _rateLimitBuffer; // Buffer for rate limiting
        private int _remainingRequests; // Tracks the remaining number of allowed requests
        private DateTime _resetTime; // The time when the rate limit will reset

        // Constructor that initializes the rate limiter with a specified buffer
        public RateLimiter(int rateLimitBuffer)
        {
            _rateLimitBuffer = rateLimitBuffer;
        }

        // Method to handle rate limiting based on Reddit's API response headers
        public async Task HandleRateLimiting(HttpResponseMessage response)
        {
            // Parse the remaining requests from the response headers
            if (response.Headers.TryGetValues("x-ratelimit-remaining", out var remainingValues))
            {
                _remainingRequests = int.Parse(remainingValues.First());
            }

            // Parse the reset time from the response headers
            if (response.Headers.TryGetValues("x-ratelimit-reset", out var resetValues))
            {
                _resetTime = DateTime.UtcNow.AddSeconds(int.Parse(resetValues.First()));
            }

            // If the remaining requests are below the buffer, wait until the rate limit resets
            if (_remainingRequests <= _rateLimitBuffer)
            {
                var delay = _resetTime - DateTime.UtcNow;
                await Task.Delay(delay);
            }
        }
    }
}
