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
        var client = new WhoopOAuthClient(ClientId, ClientSecret);

        client.Should().NotBeNull();
        client.Dispose();
    }

    [Test]
    public void Constructor_WithNullClientId_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => new WhoopOAuthClient(null!, ClientSecret))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WithEmptyClientId_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => new WhoopOAuthClient(string.Empty, ClientSecret))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WithNullClientSecret_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => new WhoopOAuthClient(ClientId, null!))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WithEmptyClientSecret_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => new WhoopOAuthClient(ClientId, string.Empty))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void BuildAuthorizationUrl_WithValidParameters_ReturnsCorrectUrl()
    {
        var redirectUri = "https://example.com/callback";
        var scope = "read:profile read:recovery";

        var url = _client.BuildAuthorizationUrl(redirectUri, scope);

        url.Should().NotBeNull();
        url.Should().StartWith("https://api.prod.whoop.com/oauth/oauth2/auth?");
        url.Should().Contain("response_type=code");
        url.Should().Contain($"client_id={Uri.EscapeDataString(ClientId)}");
        url.Should().Contain($"redirect_uri={Uri.EscapeDataString(redirectUri)}");
        url.Should().Contain($"scope={Uri.EscapeDataString(scope)}");
    }

    [Test]
    public void BuildAuthorizationUrl_WithState_IncludesStateParameter()
    {
        var redirectUri = "https://example.com/callback";
        var scope = "read:profile";
        var state = "random-state-value";

        var url = _client.BuildAuthorizationUrl(redirectUri, scope, state);

        url.Should().Contain($"state={Uri.EscapeDataString(state)}");
    }

    [Test]
    public void BuildAuthorizationUrl_WithNullRedirectUri_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => _client.BuildAuthorizationUrl(null!, "read:profile"))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void BuildAuthorizationUrl_WithEmptyScope_ThrowsArgumentException()
    {
        FluentActions.Invoking(() => _client.BuildAuthorizationUrl("https://example.com/callback", string.Empty))
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public async Task ExchangeCodeForTokenAsync_WithValidCode_ReturnsToken()
    {
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

        var result = await _client.ExchangeCodeForTokenAsync(code, redirectUri);

        result.Should().NotBeNull();
        result!.AccessToken.Should().Be(expectedToken.AccessToken);
        result.TokenType.Should().Be(expectedToken.TokenType);
        result.ExpiresIn.Should().Be(expectedToken.ExpiresIn);
        result.RefreshToken.Should().Be(expectedToken.RefreshToken);
        result.Scope.Should().Be(expectedToken.Scope);
    }

    [Test]
    public async Task ExchangeCodeForTokenAsync_SendsCorrectRequestData()
    {
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

        await _client.ExchangeCodeForTokenAsync(code, redirectUri);

        capturedContent.Should().NotBeNull();
        var formData = await capturedContent!.ReadAsStringAsync();
        formData.Should().Contain("grant_type=authorization_code");
        formData.Should().Contain($"code={code}");
        formData.Should().Contain($"redirect_uri={Uri.EscapeDataString(redirectUri)}");
        formData.Should().Contain($"client_id={ClientId}");
        formData.Should().Contain($"client_secret={ClientSecret}");
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithNullCode_ThrowsArgumentException()
    {
        FluentActions.Awaiting(async () => await _client.ExchangeCodeForTokenAsync(null!, "https://example.com/callback"))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithEmptyRedirectUri_ThrowsArgumentException()
    {
        FluentActions.Awaiting(async () => await _client.ExchangeCodeForTokenAsync("code123", string.Empty))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public void ExchangeCodeForTokenAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        SetupMockPostResponse<OAuthTokenResponse>(HttpStatusCode.Unauthorized, null);

        FluentActions.Awaiting(async () => await _client.ExchangeCodeForTokenAsync("code123", "https://example.com/callback"))
            .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsNewToken()
    {
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

        var result = await _client.RefreshTokenAsync(refreshToken);

        result.Should().NotBeNull();
        result!.AccessToken.Should().Be(expectedToken.AccessToken);
        result.RefreshToken.Should().Be(expectedToken.RefreshToken);
    }

    [Test]
    public async Task RefreshTokenAsync_SendsCorrectRequestData()
    {
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

        await _client.RefreshTokenAsync(refreshToken);

        capturedContent.Should().NotBeNull();
        var formData = await capturedContent!.ReadAsStringAsync();
        formData.Should().Contain("grant_type=refresh_token");
        formData.Should().Contain($"refresh_token={refreshToken}");
        formData.Should().Contain($"client_id={ClientId}");
        formData.Should().Contain($"client_secret={ClientSecret}");
    }

    [Test]
    public void RefreshTokenAsync_WithNullToken_ThrowsArgumentException()
    {
        FluentActions.Awaiting(async () => await _client.RefreshTokenAsync(null!))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public void RefreshTokenAsync_WithEmptyToken_ThrowsArgumentException()
    {
        FluentActions.Awaiting(async () => await _client.RefreshTokenAsync(string.Empty))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public void RefreshTokenAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        SetupMockPostResponse<OAuthTokenResponse>(HttpStatusCode.Unauthorized, null);

        FluentActions.Awaiting(async () => await _client.RefreshTokenAsync("refresh-token-123"))
            .Should().ThrowAsync<HttpRequestException>();
    }

    [Test]
    public void Dispose_WithInternalHttpClient_DisposesClient()
    {
        var client = new WhoopOAuthClient(ClientId, ClientSecret);

        FluentActions.Invoking(() => client.Dispose()).Should().NotThrow();
    }

    [Test]
    public void Dispose_WithProvidedHttpClient_DoesNotDisposeClient()
    {
        var httpClient = new HttpClient();
        var client = new WhoopOAuthClient(ClientId, ClientSecret, httpClient);

        client.Dispose();

        // Assert - HttpClient should still be usable
        FluentActions.Invoking(() => httpClient.Dispose()).Should().NotThrow();
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
