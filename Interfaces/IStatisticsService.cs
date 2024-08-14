using RedditStatsTracker.Models;

namespace RedditStatsTracker.Interfaces
{
    public interface IStatisticsService
    {
        void TrackPost(Post post);
        UserStats GetUserWithMostPosts();
        Post GetPostWithMostUpvotes();
    }
}