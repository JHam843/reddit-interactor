using RedditStatsTracker.Models;
using RedditStatsTracker.Services;
using Xunit;

namespace RedditStatsTracker.Tests
{
    public class StatisticsServiceTests
    {
        [Fact]
        public void TrackPost_ShouldUpdateUserStatsCorrectly()
        {
            // Arrange
            var service = new StatisticsService();
            var post = new Post { Author = "user1", Upvotes = 10, Title = "First Post" };

            // Act
            service.TrackPost(post);

            // Assert
            var user = service.GetUserWithMostPosts();
            Assert.Equal("user1", user.UserName);
            Assert.Equal(1, user.PostCount);
        }

        [Fact]
        public void TrackPost_ShouldUpdateTopPostCorrectly()
        {
            // Arrange
            var service = new StatisticsService();
            var post1 = new Post { Author = "user1", Upvotes = 10, Title = "First Post" };
            var post2 = new Post { Author = "user2", Upvotes = 20, Title = "Second Post" };

            // Act
            service.TrackPost(post1);
            service.TrackPost(post2);

            // Assert
            var topPost = service.GetPostWithMostUpvotes();
            Assert.Equal("Second Post", topPost.Title);
            Assert.Equal(20, topPost.Upvotes);
        }
    }
}
