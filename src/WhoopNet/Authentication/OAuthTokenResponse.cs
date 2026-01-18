using System.Text.Json.Serialization;

namespace WhoopNet.Authentication;

/// <summary>
/// Represents an OAuth 2.0 token response from the WHOOP API
/// </summary>
public class OAuthTokenResponse
{
    /// <summary>
    /// The access token to use for API requests
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// The type of token (typically "Bearer")
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// The lifetime in seconds of the access token
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// The refresh token to use to obtain a new access token
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The scope of the access token
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}
