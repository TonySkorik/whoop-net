using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a paginated response.
/// </summary>
/// <typeparam name="T">The type of items in the response.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// The list of records in this page.
    /// </summary>
    [JsonPropertyName("records")]
    public List<T>? Records { get; set; }

    /// <summary>
    /// The token to use for fetching the next page of results.
    /// </summary>
    [JsonPropertyName("next_token")]
    public string? NextToken { get; set; }
}
