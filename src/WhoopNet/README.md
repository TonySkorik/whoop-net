# WhoopNet

A .NET library for interacting with the WHOOP API. This library provides a simple and typed interface to access WHOOP fitness data including cycles, recovery metrics, workouts, sleep activities, and user profiles.

## Installation

Add the WhoopNet project to your solution or reference the compiled DLL.

## Prerequisites

- .NET 8.0 or higher
- A WHOOP developer account with API credentials
- An OAuth 2.0 access token

## Getting Started

### Authentication

To use the WHOOP API, you need to obtain an OAuth 2.0 access token. Visit the [WHOOP Developer Portal](https://developer.whoop.com/) to:

1. Create a developer account
2. Register your application
3. Obtain client credentials
4. Implement the OAuth 2.0 authorization code flow to get an access token

The following scopes are available:
- `read:profile` - Read user profile information
- `read:body_measurement` - Read body measurements
- `read:cycles` - Read physiological cycles
- `read:recovery` - Read recovery metrics
- `read:workout` - Read workout data
- `read:sleep` - Read sleep data

#### OAuth 2.0 Flow Example

```csharp
using WhoopNet.Authentication;

// Initialize the OAuth client with your credentials
var oauthClient = new WhoopOAuthClient(
    clientId: "your-client-id",
    clientSecret: "your-client-secret"
);

// Step 1: Build the authorization URL and redirect the user
var authUrl = oauthClient.BuildAuthorizationUrl(
    redirectUri: "https://your-app.com/callback",
    scope: "read:profile read:recovery read:cycles read:workout read:sleep",
    state: "random-state-string"
);
// Redirect user to authUrl...

// Step 2: After user authorizes, exchange the code for a token
var tokenResponse = await oauthClient.ExchangeCodeForTokenAsync(
    code: "authorization-code-from-callback",
    redirectUri: "https://your-app.com/callback"
);

// Step 3: Use the access token with WhoopClient
var client = new WhoopClient(tokenResponse.AccessToken);

// Step 4: When the token expires, refresh it
var newTokenResponse = await oauthClient.RefreshTokenAsync(tokenResponse.RefreshToken);
client.SetAccessToken(newTokenResponse.AccessToken);
```

### Basic Usage

```csharp
using WhoopNet;
using WhoopNet.Models;

// Initialize the client with an access token
var client = new WhoopClient("your-access-token-here");

// Get user profile
var profile = await client.GetUserProfileAsync();
Console.WriteLine($"User: {profile.FirstName} {profile.LastName}");

// Get body measurements
var measurements = await client.GetBodyMeasurementAsync();
Console.WriteLine($"Height: {measurements.HeightMeter}m, Weight: {measurements.WeightKilogram}kg");

// Get recent cycles
var cycles = await client.GetCyclesAsync(limit: 10);
foreach (var cycle in cycles.Records)
{
    Console.WriteLine($"Cycle {cycle.Id}: Strain {cycle.Score?.Strain}, Start: {cycle.Start}");
}

// Get recovery for a specific cycle
var recovery = await client.GetRecoveryAsync(cycleId: 12345);
Console.WriteLine($"Recovery Score: {recovery.Score?.Score}%");
Console.WriteLine($"HRV: {recovery.Score?.HrvRmssdMilli}ms");

// Get recent workouts
var workouts = await client.GetWorkoutsAsync(limit: 10);
foreach (var workout in workouts.Records)
{
    Console.WriteLine($"Workout {workout.Id}: Sport {workout.SportId}, Strain {workout.Score?.Strain}");
}

// Get recent sleep activities
var sleeps = await client.GetSleepsAsync(limit: 10);
foreach (var sleep in sleeps.Records)
{
    Console.WriteLine($"Sleep {sleep.Id}: Performance {sleep.Score?.SleepPerformancePercentage}%");
}
```

### Using with HttpClient Factory

For production applications, it's recommended to use `IHttpClientFactory`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using WhoopNet;

// Configure services
var services = new ServiceCollection();
services.AddHttpClient<WhoopClient>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri("https://api.prod.whoop.com");
    client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "your-access-token");
});

var serviceProvider = services.BuildServiceProvider();
var whoopClient = serviceProvider.GetRequiredService<WhoopClient>();
```

### Pagination

Many endpoints support pagination using the `nextToken` parameter:

```csharp
var firstPage = await client.GetCyclesAsync(limit: 25);

// Get next page using the token from the first page
if (!string.IsNullOrEmpty(firstPage.NextToken))
{
    var secondPage = await client.GetCyclesAsync(limit: 25, nextToken: firstPage.NextToken);
}
```

### Date Filtering

You can filter results by date range:

```csharp
var startDate = DateTime.UtcNow.AddDays(-7);
var endDate = DateTime.UtcNow;

var recentCycles = await client.GetCyclesAsync(
    start: startDate,
    end: endDate,
    limit: 50
);
```

### Activity Mapping

If you're migrating from v1 to v2 of the WHOOP API, you can map old activity IDs:

```csharp
var mapping = await client.GetActivityMappingAsync(activityV1Id: 123456);
Console.WriteLine($"V2 Activity ID: {mapping.V2ActivityId}");
```

## API Coverage

This library implements the following WHOOP API v2 endpoints:

### User Endpoints
- `GET /v2/user/profile/basic` - Get user profile
- `GET /v2/user/measurement/body` - Get body measurements

### Cycle Endpoints
- `GET /v2/cycle` - List cycles (paginated)
- `GET /v2/cycle/{cycleId}` - Get a specific cycle

### Recovery Endpoints
- `GET /v2/recovery` - List recoveries (paginated)
- `GET /v2/recovery/{cycleId}` - Get recovery for a cycle

### Workout Endpoints
- `GET /v2/activity/workout` - List workouts (paginated)
- `GET /v2/activity/workout/{workoutId}` - Get a specific workout

### Sleep Endpoints
- `GET /v2/activity/sleep` - List sleep activities (paginated)
- `GET /v2/activity/sleep/{sleepId}` - Get a specific sleep activity

### Legacy Endpoints
- `GET /v1/activity-mapping/{activityV1Id}` - Map v1 activity ID to v2 UUID

## Models

The library includes strongly-typed models for all API responses:

- `UserProfile` - User profile information
- `BodyMeasurement` - Body measurements (height, weight, max HR)
- `Cycle` - Physiological cycle data
- `Recovery` - Recovery metrics
- `Workout` - Workout/activity data
- `Sleep` - Sleep activity data
- `PaginatedResponse<T>` - Paginated API responses

All models use JSON serialization attributes for proper API communication.

## Error Handling

The client will throw exceptions for failed HTTP requests. Wrap your calls in try-catch blocks:

```csharp
try
{
    var profile = await client.GetUserProfileAsync();
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"API request failed: {ex.Message}");
}
```

## Resources

- [WHOOP Developer Portal](https://developer.whoop.com/)
- [WHOOP API Documentation](https://developer.whoop.com/api/)
- [OpenAPI Specification](https://api.prod.whoop.com/developer/doc/openapi.json)

## License

This project follows the repository's license terms.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.
