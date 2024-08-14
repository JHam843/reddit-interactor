using System.Collections.Generic;

namespace RedditStatsTracker.Models
{
    public class RedditResponse
    {
        public RedditData Data { get; set; }  // Wrapper for the actual data
    }

    public class RedditData
    {
        public List<RedditChild> Children { get; set; }  // List of post data
    }

    public class RedditChild
    {
        public Post Data { get; set; }  // Actual post data
    }
}