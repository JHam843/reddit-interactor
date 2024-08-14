# reddit-interactor

RedditStatsTracker is a .NET console application that tracks statistics from a specified subreddit in near real-time. It retrieves the latest posts and provides insights such as the post with the most upvotes and the user with the most posts.

## Features
- Fetches and processes posts from a subreddit in near real-time.
- Tracks and displays the post with the most upvotes.
- Tracks and displays the user with the most posts.
- Handles Reddit API rate limiting.
- Configurable via `appsettings.json`.

## Installation and Setup

### Prerequisites
- .NET 6.0 SDK installed on your machine.
- A Reddit API account with a valid `ClientId` and `ClientSecret`.

### Cloning the Repository
To clone the repository, run:
```bash
git clone https://github.com/JHam843/reddit-interactor
cd reddit-interactor/RedditStatsTracker


dotnet restore

#### 3. Configuration

```markdown
## Configuration

### appsettings.json

The `appsettings.json` file contains the necessary configurations for the application to interact with the Reddit API.

```json
{
  "Reddit": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "UserAgent": "YourAppName/1.0 (by /u/YourRedditUsername)",
    "Subreddit": "your-subreddit",
    "RateLimitBuffer": 5
  }
}

#### 4. Running the Application

```markdown
## Running the Application

To run the application, use the following command:
```bash
dotnet run