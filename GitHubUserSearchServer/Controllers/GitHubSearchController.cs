using GitHubUserSearchApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GitHubUserSearchApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubSearchController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GitHubSearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitHubUserSearchApi", "1.0"));

            var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            Console.WriteLine(githubToken);
            if (!string.IsNullOrEmpty(githubToken))
            {   
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {githubToken}");
            }
        }

        [HttpGet("search_github_users")]
        public async Task<IActionResult> SearchGitHubUsers([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int per_page = 12)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest("Query parameter 'q' is required.");
            }

            try
            {
                var searchUrl = $"https://api.github.com/search/users?q={q}&page={page}&per_page={per_page}";
                var searchResponse = await _httpClient.GetAsync(searchUrl);

                if (!searchResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)searchResponse.StatusCode, "GitHub search request failed.");
                }

                var searchContent = await searchResponse.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<GitHubSearchResult>(searchContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (searchResult?.Items == null)
                {
                    return NotFound("No users found in the GitHub API response.");
                }

                var userTasks = searchResult.Items.Select(async item =>
                {
                    if (!item.Login.Contains(q, StringComparison.OrdinalIgnoreCase)) return null;
                    var userUrl = $"https://api.github.com/users/{item.Login}";
                    var userResponse = await _httpClient.GetAsync(userUrl);
                    if (!userResponse.IsSuccessStatusCode) return null;

                    var userContent = await userResponse.Content.ReadAsStringAsync();
                    var userDetails = JsonSerializer.Deserialize<GitHubUserDetail>(userContent);

                    return new GitHubUser
                    {
                        Username = item.Login,
                        Image = item.AvatarUrl ?? "",
                        PublicRepos = userDetails?.PublicRepos ?? 0
                    };
                });

                var users = await Task.WhenAll(userTasks);
                return Ok(new { total_count = searchResult.TotalCount, users = users.Where(u => u != null) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
