#nullable disable
using System.Text.Json.Serialization;
namespace GitHubUserSearchApi.Models
{
    public class GitHubSearchResult
    {
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("items")]
        public List<GitHubUserItem> Items { get; set; }
    }
    
    public class GitHubUserItem
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
    }
    
    public class GitHubUserDetail
    {
        [JsonPropertyName("public_repos")]
        public int PublicRepos { get; set; }
    }
    
    public class GitHubUser
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public int PublicRepos { get; set; }
    }
}