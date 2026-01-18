using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a user's basic profile information
/// </summary>
public class UserProfile
{
    /// <summary>
    /// The user's unique identifier
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The user's email address
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The user's first name
    /// </summary>
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// The user's last name
    /// </summary>
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}
