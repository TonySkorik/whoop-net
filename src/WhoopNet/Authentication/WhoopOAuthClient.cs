using System.Net.Http.Json;

namespace WhoopNet.Authentication;

/// <summary>
/// Helper class for OAuth 2.0 authentication with the WHOOP API
/// </summary>
public class WhoopOAuthClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly bool _disposeHttpClient;

    private const string AuthorizationUrl = "https://api.prod.whoop.com/oauth/oauth2/auth";
    private const string TokenUrl = "https://api.prod.whoop.com/oauth/oauth2/token";

    /// <summary>
    /// Initializes a new instance of the WhoopOAuthClient class
    /// </summary>
    /// <param name="clientId">The OAuth 2.0 client ID</param>
    /// <param name="clientSecret">The OAuth 2.0 client secret</param>
    /// <param name="httpClient">Optional HttpClient to use for requests</param>
    public WhoopOAuthClient(string clientId, string clientSecret, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(clientId))
            throw new ArgumentException("Client ID cannot be null or empty", nameof(clientId));
        
        if (string.IsNullOrWhiteSpace(clientSecret))
            throw new ArgumentException("Client secret cannot be null or empty", nameof(clientSecret));

        _clientId = clientId;
        _clientSecret = clientSecret;

        if (httpClient != null)
        {
            _httpClient = httpClient;
            _disposeHttpClient = false;
        }
        else
        {
            _httpClient = new HttpClient();
            _disposeHttpClient = true;
        }
    }

    /// <summary>
    /// Builds the authorization URL for the OAuth 2.0 authorization code flow
    /// </summary>
    /// <param name="redirectUri">The redirect URI registered with your WHOOP app</param>
    /// <param name="scope">Space-separated list of scopes (e.g., "read:profile read:recovery")</param>
    /// <param name="state">Optional state parameter for CSRF protection</param>
    /// <returns>The authorization URL to redirect the user to</returns>
    public string BuildAuthorizationUrl(string redirectUri, string scope, string? state = null)
    {
        if (string.IsNullOrWhiteSpace(redirectUri))
            throw new ArgumentException("Redirect URI cannot be null or empty", nameof(redirectUri));
        
        if (string.IsNullOrWhiteSpace(scope))
            throw new ArgumentException("Scope cannot be null or empty", nameof(scope));

        var queryParams = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = _clientId,
            ["redirect_uri"] = redirectUri,
            ["scope"] = scope
        };

        if (!string.IsNullOrWhiteSpace(state))
        {
            queryParams["state"] = state;
        }

        var queryString = string.Join("&", queryParams.Select(kvp => 
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

        return $"{AuthorizationUrl}?{queryString}";
    }

    /// <summary>
    /// Exchanges an authorization code for an access token
    /// </summary>
    /// <param name="code">The authorization code received from the authorization endpoint</param>
    /// <param name="redirectUri">The same redirect URI used in the authorization request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The OAuth token response containing the access token</returns>
    public async Task<OAuthTokenResponse?> ExchangeCodeForTokenAsync(
        string code, 
        string redirectUri, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Authorization code cannot be null or empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(redirectUri))
            throw new ArgumentException("Redirect URI cannot be null or empty", nameof(redirectUri));

        var requestData = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret
        };

        var content = new FormUrlEncodedContent(requestData);
        var response = await _httpClient.PostAsync(TokenUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OAuthTokenResponse>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The OAuth token response containing the new access token</returns>
    public async Task<OAuthTokenResponse?> RefreshTokenAsync(
        string refreshToken, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

        var requestData = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret
        };

        var content = new FormUrlEncodedContent(requestData);
        var response = await _httpClient.PostAsync(TokenUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OAuthTokenResponse>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Disposes the HttpClient if it was created internally
    /// </summary>
    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient?.Dispose();
        }
    }
}
