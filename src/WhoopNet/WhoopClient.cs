using System.Net.Http.Headers;
using System.Net.Http.Json;
using WhoopNet.Models;

namespace WhoopNet;

/// <summary>
/// Client for interacting with the WHOOP API
/// </summary>
public class WhoopClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;
    private const string BaseUrl = "https://api.prod.whoop.com";

    /// <summary>
    /// Initializes a new instance of the WhoopClient class with a custom HttpClient
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for API requests</param>
    public WhoopClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _disposeHttpClient = false;

        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }
    }

    /// <summary>
    /// Initializes a new instance of the WhoopClient class with an access token
    /// </summary>
    /// <param name="accessToken">The OAuth 2.0 access token</param>
    public WhoopClient(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _disposeHttpClient = true;
    }

    /// <summary>
    /// Sets the access token for authentication
    /// </summary>
    /// <param name="accessToken">The OAuth 2.0 access token</param>
    public void SetAccessToken(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    /// <summary>
    /// Helper method to perform GET request and deserialize JSON response
    /// </summary>
    private async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Helper method to build query string from pagination and filter parameters
    /// </summary>
    private static string BuildQueryString(int? limit, DateTime? start, DateTime? end, string? nextToken)
    {
        var queryParams = new List<string>();
        
        if (limit.HasValue)
            queryParams.Add($"limit={limit.Value}");
        
        if (start.HasValue)
            queryParams.Add($"start={start.Value:yyyy-MM-ddTHH:mm:ss.fffZ}");
        
        if (end.HasValue)
            queryParams.Add($"end={end.Value:yyyy-MM-ddTHH:mm:ss.fffZ}");
        
        if (!string.IsNullOrEmpty(nextToken))
            queryParams.Add($"nextToken={Uri.EscapeDataString(nextToken)}");

        return queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
    }

    /// <summary>
    /// Gets the user's basic profile information
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user's profile information</returns>
    public Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken = default) =>
        GetAsync<UserProfile>("/v2/user/profile/basic", cancellationToken);

    /// <summary>
    /// Gets the user's body measurements
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user's body measurements</returns>
    public Task<BodyMeasurement?> GetBodyMeasurementAsync(CancellationToken cancellationToken = default) =>
        GetAsync<BodyMeasurement>("/v2/user/measurement/body", cancellationToken);

    /// <summary>
    /// Gets a specific cycle by ID
    /// </summary>
    /// <param name="cycleId">The cycle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cycle information</returns>
    public Task<Cycle?> GetCycleAsync(int cycleId, CancellationToken cancellationToken = default) =>
        GetAsync<Cycle>($"/v2/cycle/{cycleId}", cancellationToken);

    /// <summary>
    /// Gets a paginated list of cycles for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing cycles</returns>
    public Task<PaginatedResponse<Cycle>?> GetCyclesAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(limit, start, end, nextToken);
        return GetAsync<PaginatedResponse<Cycle>>($"/v2/cycle{queryString}", cancellationToken);
    }

    /// <summary>
    /// Gets a specific recovery by cycle ID
    /// </summary>
    /// <param name="cycleId">The cycle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The recovery information</returns>
    public Task<Recovery?> GetRecoveryAsync(int cycleId, CancellationToken cancellationToken = default) =>
        GetAsync<Recovery>($"/v2/recovery/{cycleId}", cancellationToken);

    /// <summary>
    /// Gets a paginated list of recoveries for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing recoveries</returns>
    public Task<PaginatedResponse<Recovery>?> GetRecoveriesAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(limit, start, end, nextToken);
        return GetAsync<PaginatedResponse<Recovery>>($"/v2/recovery{queryString}", cancellationToken);
    }

    /// <summary>
    /// Gets a specific workout by ID
    /// </summary>
    /// <param name="workoutId">The workout ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The workout information</returns>
    public Task<Workout?> GetWorkoutAsync(int workoutId, CancellationToken cancellationToken = default) =>
        GetAsync<Workout>($"/v2/activity/workout/{workoutId}", cancellationToken);

    /// <summary>
    /// Gets a paginated list of workouts for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing workouts</returns>
    public Task<PaginatedResponse<Workout>?> GetWorkoutsAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(limit, start, end, nextToken);
        return GetAsync<PaginatedResponse<Workout>>($"/v2/activity/workout{queryString}", cancellationToken);
    }

    /// <summary>
    /// Gets a specific sleep by ID
    /// </summary>
    /// <param name="sleepId">The sleep ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sleep information</returns>
    public Task<Sleep?> GetSleepAsync(int sleepId, CancellationToken cancellationToken = default) =>
        GetAsync<Sleep>($"/v2/activity/sleep/{sleepId}", cancellationToken);

    /// <summary>
    /// Gets a paginated list of sleep activities for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing sleep activities</returns>
    public Task<PaginatedResponse<Sleep>?> GetSleepsAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = BuildQueryString(limit, start, end, nextToken);
        return GetAsync<PaginatedResponse<Sleep>>($"/v2/activity/sleep{queryString}", cancellationToken);
    }

    /// <summary>
    /// Gets the v2 UUID for a given v1 activity ID
    /// </summary>
    /// <param name="activityV1Id">The v1 activity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The activity mapping information</returns>
    public Task<ActivityMapping?> GetActivityMappingAsync(int activityV1Id, CancellationToken cancellationToken = default) =>
        GetAsync<ActivityMapping>($"/v1/activity-mapping/{activityV1Id}", cancellationToken);

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
