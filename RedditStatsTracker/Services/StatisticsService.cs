using RedditStatsTracker.Interfaces;
using RedditStatsTracker.Models;

namespace RedditStatsTracker.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly Dictionary<string, UserStats> _userStats = new(); // Tracks user statistics
        private Post _postWithMostUpvotes; // Tracks the post with the most upvotes

        // Method to track statistics for a given post
        public void TrackPost(Post post)
        {
            // Update or add the user's statistics
            if (!_userStats.ContainsKey(post.Author))
            {
                _userStats[post.Author] = new UserStats { UserName = post.Author };
            }

            _userStats[post.Author].PostCount++;

            // Update the post with the most upvotes if necessary
            if (_postWithMostUpvotes == null || post.Upvotes > _postWithMostUpvotes.Upvotes)
            {
                _postWithMostUpvotes = post;
            }
        }

        // Method to get the user with the most posts
        public UserStats GetUserWithMostPosts()
        {
            return _userStats.Values.OrderByDescending(u => u.PostCount).FirstOrDefault();
        }

        // Method to get the post with the most upvotes
        public Post GetPostWithMostUpvotes()
        {
            return _postWithMostUpvotes;
        }
    }
}