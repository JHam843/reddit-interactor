using Xunit;
using RedditStatsTracker.Services;
using RedditStatsTracker.Models;

public class StatisticsServiceTests
{
    [Fact]
    public void TrackPost_UpdatesStatisticsCorrectly()
    {
        var service = new StatisticsService();
        var post = new Post { Author = "user1", Upvotes = 100,Title = "Sample Post" };
          service.TrackPost(post);

       var topUser = service.GetUserWithMostPosts();
       var topPost = service.GetPostWithMostUpvotes();

       Assert.Equal("user1", topUser.UserName);
       Assert.Equal("Sample Post", topPost.Title);
   }
}