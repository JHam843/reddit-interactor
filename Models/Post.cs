namespace RedditStatsTracker.Models
{
    public class Post
    {
        public string Id { get; set; }            // Unique identifier for the post
        public string Title { get; set; }         // Title of the post
        public string Author { get; set; }        // Author of the post
        public int Upvotes { get; set; }          // Number of upvotes the post has received
        public string Subreddit { get; set; }     // Subreddit where the post was made
        public DateTime CreatedUtc { get; set; }  // Creation time in UTC
    }
}