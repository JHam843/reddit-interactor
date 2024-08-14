using RedditStatsTracker.Models;

namespace RedditStatsTracker.Interfaces
{
    public interface IRedditService
    {
        Task<IEnumerable<Post>> GetRecentPostsAsync(string subreddit);
    }
}