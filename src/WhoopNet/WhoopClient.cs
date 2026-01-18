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
    /// Gets the user's basic profile information
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user's profile information</returns>
    public async Task<UserProfile?> GetUserProfileAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("/v2/user/profile/basic", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserProfile>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the user's body measurements
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user's body measurements</returns>
    public async Task<BodyMeasurement?> GetBodyMeasurementAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("/v2/user/measurement/body", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BodyMeasurement>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a specific cycle by ID
    /// </summary>
    /// <param name="cycleId">The cycle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cycle information</returns>
    public async Task<Cycle?> GetCycleAsync(int cycleId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v2/cycle/{cycleId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Cycle>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of cycles for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing cycles</returns>
    public async Task<PaginatedResponse<Cycle>?> GetCyclesAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
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

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var response = await _httpClient.GetAsync($"/v2/cycle{queryString}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaginatedResponse<Cycle>>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a specific recovery by cycle ID
    /// </summary>
    /// <param name="cycleId">The cycle ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The recovery information</returns>
    public async Task<Recovery?> GetRecoveryAsync(int cycleId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v2/recovery/{cycleId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Recovery>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of recoveries for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing recoveries</returns>
    public async Task<PaginatedResponse<Recovery>?> GetRecoveriesAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
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

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var response = await _httpClient.GetAsync($"/v2/recovery{queryString}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaginatedResponse<Recovery>>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a specific workout by ID
    /// </summary>
    /// <param name="workoutId">The workout ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The workout information</returns>
    public async Task<Workout?> GetWorkoutAsync(int workoutId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v2/activity/workout/{workoutId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Workout>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of workouts for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing workouts</returns>
    public async Task<PaginatedResponse<Workout>?> GetWorkoutsAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
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

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var response = await _httpClient.GetAsync($"/v2/activity/workout{queryString}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaginatedResponse<Workout>>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a specific sleep by ID
    /// </summary>
    /// <param name="sleepId">The sleep ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sleep information</returns>
    public async Task<Sleep?> GetSleepAsync(int sleepId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v2/activity/sleep/{sleepId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Sleep>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets a paginated list of sleep activities for the user
    /// </summary>
    /// <param name="limit">Maximum number of records to return (default: 25)</param>
    /// <param name="start">Start date for filtering (ISO 8601 format)</param>
    /// <param name="end">End date for filtering (ISO 8601 format)</param>
    /// <param name="nextToken">Token for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response containing sleep activities</returns>
    public async Task<PaginatedResponse<Sleep>?> GetSleepsAsync(
        int? limit = null,
        DateTime? start = null,
        DateTime? end = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
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

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var response = await _httpClient.GetAsync($"/v2/activity/sleep{queryString}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PaginatedResponse<Sleep>>(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the v2 UUID for a given v1 activity ID
    /// </summary>
    /// <param name="activityV1Id">The v1 activity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The activity mapping information</returns>
    public async Task<ActivityMapping?> GetActivityMappingAsync(int activityV1Id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/v1/activity-mapping/{activityV1Id}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ActivityMapping>(cancellationToken: cancellationToken);
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
