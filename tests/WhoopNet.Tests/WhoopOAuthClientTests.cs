using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using WhoopNet.Authentication;

namespace WhoopNet.Tests;

[TestFixture]
public class WhoopOAuthClientTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    private HttpClient _httpClient = null!;
    private WhoopOAuthClient _client = null!;
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _client = new WhoopOAuthClient(ClientId, ClientSecret, _httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _httpClient?.Dispose();
    }

    [Test]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Act
        var client = new WhoopOAuthClient(ClientId, ClientSecret);

        // Assert
        Assert.That(client, Is.Not.Null);
        client.Dispose();
    }

    [Test]
    public void Constructor_WithNullClientId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WhoopOAuthClient(null!, ClientSecret));
    }

    [Test]
    public void Constructor_WithEmptyClientId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WhoopOAuthClient(string.Empty, ClientSecret));
    }

    [Test]
    public void Constructor_WithNullClientSecret_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WhoopOAuthClient(ClientId, null!));
    }

    [Test]
    public void Constructor_WithEmptyClientSecret_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WhoopOAuthClient(ClientId, string.Empty));
    }

    [Test]
    public void BuildAuthorizationUrl_WithValidParameters_ReturnsCorrectUrl()
    {
        // Arrange
        var redirectUri = "https://example.com/callback";
        var scope = "read:profile read:recovery";

        // Act
        var url = _client.BuildAuthorizationUrl(redirectUri, scope);

        // Assert
        Assert.That(url, Is.Not.Null);
        Assert.That(url, Does.StartWith("https://api.prod.whoop.com/oauth/oauth2/auth?"));
        Assert.That(url, Does.Contain("response_type=code"));
        Assert.That(url, Does.Contain($"client_id={Uri.EscapeDataString(ClientId)}"));
        Assert.That(url, Does.Contain($"redirect_uri={Uri.EscapeDataString(redirectUri)}"));
        Assert.That(url, Does.Contain($"scope={Uri.EscapeDataString(scope)}"));
    }

    [Test]
    public void BuildAuthorizationUrl_WithState_IncludesStateParameter()
    {
        // Arrange
        var redirectUri = "https://example.com/callback";
        var scope = "read:profile";
        var state = "random-state-value";

        // Act
        var url = _client.BuildAuthorizationUrl(redirectUri, scope, state);

        // Assert
        Assert.That(url, Does.Contain($"state={Uri.EscapeDataString(state)}"));
    }

    [Test]
    public void BuildAuthorizationUrl_WithNullRedirectUri_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _client.BuildAuthorizationUrl(null!, "read:profile"));
    }

    [Test]
    public void BuildAuthorizationUrl_WithEmptyScope_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _client.BuildAuthorizationUrl("https://example.com/callback", string.Empty));
    }

    [Test]
    public async Task ExchangeCodeForTokenAsync_WithValidCode_ReturnsToken()
    {
        // Arrange
        var code = "auth-code-123";
        var redirectUri = "https://example.com/callback";
        var expectedToken = new OAuthTokenResponse
        {
            AccessToken = "access-token-123",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            RefreshToken = "refresh-token-123",
            Scope = "read:profile read:recovery"
        };

        SetupMockPostResponse(HttpStatusCode.OK, expectedToken);

        // Act
        var result = await _client.ExchangeCodeForTokenAsync(code, redirectUri);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.AccessToken, Is.EqualTo(expectedToken.AccessToken));
        Assert.That(result.TokenType, Is.EqualTo(expectedToken.TokenType));
        Assert.That(result.ExpiresIn, Is.EqualTo(expectedToken.ExpiresIn));
        Assert.That(result.RefreshToken, Is.EqualTo(expectedToken.RefreshToken));
        Assert.That(result.Scope, Is.EqualTo(expectedToken.Scope));
    }

    [Test]
    public async Task ExchangeCodeForTokenAsync_SendsCorrectRequestData()
    {
        // Arrange
        var code = "auth-code-123";
        var redirectUri = "https://example.com/callback";
        var expectedToken = new OAuthTokenResponse { AccessToken = "token" };

        FormUrlEncodedContent? capturedContent = null;
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>(async (request, _) =>
            {
                if (request.Content is FormUrlEncodedContent content)
                {
                    capturedContent = content;
                }
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedToken)
            });

        // Act
        await _client.ExchangeCodeForTokenAsync(code, redirectUri);

        // Assert
        Assert.That(capturedContent, Is.Not.Null);
        var formData = await capturedContent!.ReadAsStringAsync();
        Assert.That(formData, Does.Contain("grant_type=authorization_code"));
        Assert.That(formData, Does.Contain($"code={code}"));
        Assert.That(formData, Does.Contain($"redirect_uri={Uri.EscapeDataString(redirectUri)}"));
        Assert.That(formData, Does.Contain($"client_id={ClientId}"));
        Assert.That(formData, Does.Contain($"client_secret={ClientSecret}"));
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithNullCode_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => 
            await _client.ExchangeCodeForTokenAsync(null!, "https://example.com/callback"));
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithEmptyRedirectUri_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => 
            await _client.ExchangeCodeForTokenAsync("code123", string.Empty));
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        // Arrange
        SetupMockPostResponse<OAuthTokenResponse>(HttpStatusCode.Unauthorized, null);

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => 
            await _client.ExchangeCodeForTokenAsync("code123", "https://example.com/callback"));
    }

    [Test]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsNewToken()
    {
        // Arrange
        var refreshToken = "refresh-token-123";
        var expectedToken = new OAuthTokenResponse
        {
            AccessToken = "new-access-token-456",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            RefreshToken = "new-refresh-token-456",
            Scope = "read:profile"
        };

        SetupMockPostResponse(HttpStatusCode.OK, expectedToken);

        // Act
        var result = await _client.RefreshTokenAsync(refreshToken);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.AccessToken, Is.EqualTo(expectedToken.AccessToken));
        Assert.That(result.RefreshToken, Is.EqualTo(expectedToken.RefreshToken));
    }

    [Test]
    public async Task RefreshTokenAsync_SendsCorrectRequestData()
    {
        // Arrange
        var refreshToken = "refresh-token-123";
        var expectedToken = new OAuthTokenResponse { AccessToken = "token" };

        FormUrlEncodedContent? capturedContent = null;
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>(async (request, _) =>
            {
                if (request.Content is FormUrlEncodedContent content)
                {
                    capturedContent = content;
                }
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedToken)
            });

        // Act
        await _client.RefreshTokenAsync(refreshToken);

        // Assert
        Assert.That(capturedContent, Is.Not.Null);
        var formData = await capturedContent!.ReadAsStringAsync();
        Assert.That(formData, Does.Contain("grant_type=refresh_token"));
        Assert.That(formData, Does.Contain($"refresh_token={refreshToken}"));
        Assert.That(formData, Does.Contain($"client_id={ClientId}"));
        Assert.That(formData, Does.Contain($"client_secret={ClientSecret}"));
    }

    [Test]
    public void RefreshTokenAsync_WithNullToken_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => 
            await _client.RefreshTokenAsync(null!));
    }

    [Test]
    public void RefreshTokenAsync_WithEmptyToken_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => 
            await _client.RefreshTokenAsync(string.Empty));
    }

    [Test]
    public void RefreshTokenAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        // Arrange
        SetupMockPostResponse<OAuthTokenResponse>(HttpStatusCode.Unauthorized, null);

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => 
            await _client.RefreshTokenAsync("refresh-token-123"));
    }

    [Test]
    public void Dispose_WithInternalHttpClient_DisposesClient()
    {
        // Arrange
        var client = new WhoopOAuthClient(ClientId, ClientSecret);

        // Act & Assert
        Assert.DoesNotThrow(() => client.Dispose());
    }

    [Test]
    public void Dispose_WithProvidedHttpClient_DoesNotDisposeClient()
    {
        // Arrange
        var httpClient = new HttpClient();
        var client = new WhoopOAuthClient(ClientId, ClientSecret, httpClient);

        // Act
        client.Dispose();

        // Assert - HttpClient should still be usable
        Assert.DoesNotThrow(() => httpClient.Dispose());
    }

    private void SetupMockPostResponse<T>(HttpStatusCode statusCode, T? content)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = content != null ? JsonContent.Create(content) : null
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }
}
