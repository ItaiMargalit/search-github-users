using System.Text.Json.Serialization;

namespace GitHubUserSearchApi.Models
{
    // Model for GitHub search result
    public class GitHubSearchResult
    {
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("items")]
        public List<GitHubUserItem> Items { get; set; }
    }

    // Model for individual GitHub user in search results
    public class GitHubUserItem
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
    }

    // Model for detailed GitHub user information
    public class GitHubUserDetail
    {
        [JsonPropertyName("public_repos")]
        public int PublicRepos { get; set; }
    }

    // Final model to return GitHub user data to the frontend
    public class GitHubUser
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public int PublicRepos { get; set; }
    }
}