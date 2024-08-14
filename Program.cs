using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedditStatsTracker.Interfaces;
using RedditStatsTracker.Models;
using RedditStatsTracker.Services;
using RedditStatsTracker.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditStatsTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load configuration from appsettings.json
            // This configuration file contains Reddit API credentials and settings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Map the Reddit section of the config file to the RedditConfig class
            var redditConfig = config.GetSection("Reddit").Get<RedditConfig>();
            Console.WriteLine(redditConfig);

            // Setup dependency injection (DI) container
            // DI helps manage the lifecycle of objects and their dependencies
            var services = new ServiceCollection()
                .AddSingleton<HttpClient>() // Registers HttpClient as a singleton service
                .AddSingleton<IRedditService, RedditService>() // Registers RedditService under the IRedditService interface
                .AddSingleton<IStatisticsService, StatisticsService>() // Registers StatisticsService under the IStatisticsService interface
                .AddSingleton(provider => new RateLimiter(redditConfig.RateLimitBuffer)) // Registers RateLimiter with a parameter from the config
                .AddSingleton(redditConfig) // Registers the RedditConfig object itself
                .BuildServiceProvider(); // Builds the service provider

            // Resolve services from the DI container
            var redditService = services.GetRequiredService<IRedditService>();
            var statisticsService = services.GetRequiredService<IStatisticsService>();
            var rateLimiter = services.GetRequiredService<RateLimiter>();

            Console.WriteLine($"Tracking subreddit: {redditConfig.Subreddit}");

            // Main loop: Continuously fetch posts, track statistics, and display results
            while (true)
            {
                try
                {
                    // Fetch recent posts from the subreddit
                    var posts = await redditService.GetRecentPostsAsync(redditConfig.Subreddit);

                    foreach (var post in posts)
                    {
                        // Track each post's statistics
                        statisticsService.TrackPost(post);
                    }

                    // Get and display the post with the most upvotes and the user with the most posts
                    var topPost = statisticsService.GetPostWithMostUpvotes();
                    var topUser = statisticsService.GetUserWithMostPosts();

                    Console.WriteLine($"Top Post: {topPost?.Title} by {topPost?.Author} with {topPost?.Upvotes} upvotes.");
                    Console.WriteLine($"Top User: {topUser?.UserName} with {topUser?.PostCount} posts.");

                    // Delay for 1 minute before fetching the next batch of posts
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                catch (Exception ex)
                {
                    // Handle and log any exceptions that occur during execution
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
