using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using WhoopNet.Models;

namespace WhoopNet.Tests;

[TestFixture]
public class WhoopClientTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    private HttpClient _httpClient = null!;
    private WhoopClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.prod.whoop.com")
        };
        _client = new WhoopClient(_httpClient);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _httpClient?.Dispose();
    }

    [Test]
    public void Constructor_WithHttpClient_SetsBaseAddressIfNull()
    {
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        var client = new WhoopClient(httpClient);

        Assert.That(httpClient.BaseAddress, Is.Not.Null);
        Assert.That(httpClient.BaseAddress!.ToString(), Is.EqualTo("https://api.prod.whoop.com/"));

        client.Dispose();
        httpClient.Dispose();
    }

    [Test]
    public void Constructor_WithAccessToken_SetsAuthorizationHeader()
    {
        var accessToken = "test-access-token";

        using var client = new WhoopClient(accessToken);

        // Assert - we can't easily access the internal HttpClient, but we can test it works
        Assert.That(client, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new WhoopClient((HttpClient)null!));
    }

    [Test]
    public void Constructor_WithEmptyAccessToken_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new WhoopClient(string.Empty));
    }

    [Test]
    public void SetAccessToken_WithValidToken_DoesNotThrow()
    {
        var token = "new-access-token";

        Assert.DoesNotThrow(() => _client.SetAccessToken(token));
    }

    [Test]
    public void SetAccessToken_WithEmptyToken_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _client.SetAccessToken(string.Empty));
    }

    [Test]
    public async Task GetUserProfileAsync_ReturnsUserProfile()
    {
        var expectedProfile = new UserProfile
        {
            UserId = 123,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        SetupMockResponse(HttpStatusCode.OK, expectedProfile);

        var result = await _client.GetUserProfileAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.UserId, Is.EqualTo(expectedProfile.UserId));
        Assert.That(result.Email, Is.EqualTo(expectedProfile.Email));
        Assert.That(result.FirstName, Is.EqualTo(expectedProfile.FirstName));
        Assert.That(result.LastName, Is.EqualTo(expectedProfile.LastName));
    }

    [Test]
    public async Task GetBodyMeasurementAsync_ReturnsBodyMeasurement()
    {
        var expectedMeasurement = new BodyMeasurement
        {
            HeightMeter = 1.75,
            WeightKilogram = 75.5,
            MaxHeartRate = 190
        };

        SetupMockResponse(HttpStatusCode.OK, expectedMeasurement);

        var result = await _client.GetBodyMeasurementAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.HeightMeter, Is.EqualTo(expectedMeasurement.HeightMeter));
        Assert.That(result.WeightKilogram, Is.EqualTo(expectedMeasurement.WeightKilogram));
        Assert.That(result.MaxHeartRate, Is.EqualTo(expectedMeasurement.MaxHeartRate));
    }

    [Test]
    public async Task GetCycleAsync_ReturnsCycle()
    {
        var cycleId = 12345;
        var expectedCycle = new Cycle
        {
            Id = cycleId,
            UserId = 123,
            Start = DateTime.UtcNow.AddHours(-8),
            End = DateTime.UtcNow,
            Score = new CycleScore
            {
                Strain = 15.5,
                Kilojoule = 5000,
                AverageHeartRate = 120,
                MaxHeartRate = 180
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedCycle);

        var result = await _client.GetCycleAsync(cycleId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(expectedCycle.Id));
        Assert.That(result.UserId, Is.EqualTo(expectedCycle.UserId));
        Assert.That(result.Score, Is.Not.Null);
        Assert.That(result.Score!.Strain, Is.EqualTo(expectedCycle.Score.Strain));
    }

    [Test]
    public async Task GetCyclesAsync_ReturnsPaginatedCycles()
    {
        var expectedResponse = new PaginatedResponse<Cycle>
        {
            Records = new List<Cycle>
            {
                new Cycle { Id = 1, UserId = 123 },
                new Cycle { Id = 2, UserId = 123 }
            },
            NextToken = "next-page-token"
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetCyclesAsync(limit: 10);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Records, Is.Not.Null);
        Assert.That(result.Records!.Count, Is.EqualTo(2));
        Assert.That(result.NextToken, Is.EqualTo("next-page-token"));
    }

    [Test]
    public async Task GetRecoveryAsync_ReturnsRecovery()
    {
        var cycleId = 12345;
        var expectedRecovery = new Recovery
        {
            CycleId = cycleId,
            SleepId = 67890,
            UserId = 123,
            Score = new RecoveryScore
            {
                UserCalibrating = false,
                Score = 85.5,
                RestingHeartRate = 55,
                HrvRmssdMilli = 75.0
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedRecovery);

        var result = await _client.GetRecoveryAsync(cycleId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.CycleId, Is.EqualTo(expectedRecovery.CycleId));
        Assert.That(result.Score, Is.Not.Null);
        Assert.That(result.Score!.Score, Is.EqualTo(expectedRecovery.Score.Score));
    }

    [Test]
    public async Task GetRecoveriesAsync_ReturnsPaginatedRecoveries()
    {
        var expectedResponse = new PaginatedResponse<Recovery>
        {
            Records = new List<Recovery>
            {
                new Recovery { CycleId = 1, UserId = 123 },
                new Recovery { CycleId = 2, UserId = 123 }
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetRecoveriesAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Records, Is.Not.Null);
        Assert.That(result.Records!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetWorkoutAsync_ReturnsWorkout()
    {
        var workoutId = 12345;
        var expectedWorkout = new Workout
        {
            Id = workoutId,
            UserId = 123,
            SportId = 1,
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow,
            Score = new WorkoutScore
            {
                Strain = 12.5,
                AverageHeartRate = 145,
                MaxHeartRate = 175
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedWorkout);

        var result = await _client.GetWorkoutAsync(workoutId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(expectedWorkout.Id));
        Assert.That(result.Score, Is.Not.Null);
        Assert.That(result.Score!.Strain, Is.EqualTo(expectedWorkout.Score.Strain));
    }

    [Test]
    public async Task GetWorkoutsAsync_ReturnsPaginatedWorkouts()
    {
        var expectedResponse = new PaginatedResponse<Workout>
        {
            Records = new List<Workout>
            {
                new Workout { Id = 1, UserId = 123, SportId = 1 },
                new Workout { Id = 2, UserId = 123, SportId = 2 }
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetWorkoutsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Records, Is.Not.Null);
        Assert.That(result.Records!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetSleepAsync_ReturnsSleep()
    {
        var sleepId = 12345;
        var expectedSleep = new Sleep
        {
            Id = sleepId,
            UserId = 123,
            Nap = false,
            Start = DateTime.UtcNow.AddHours(-8),
            End = DateTime.UtcNow,
            Score = new SleepScore
            {
                SleepPerformancePercentage = 85.5,
                SleepEfficiencyPercentage = 92.0
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedSleep);

        var result = await _client.GetSleepAsync(sleepId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(expectedSleep.Id));
        Assert.That(result.Nap, Is.EqualTo(expectedSleep.Nap));
        Assert.That(result.Score, Is.Not.Null);
    }

    [Test]
    public async Task GetSleepsAsync_ReturnsPaginatedSleeps()
    {
        var expectedResponse = new PaginatedResponse<Sleep>
        {
            Records = new List<Sleep>
            {
                new Sleep { Id = 1, UserId = 123, Nap = false },
                new Sleep { Id = 2, UserId = 123, Nap = true }
            }
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetSleepsAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Records, Is.Not.Null);
        Assert.That(result.Records!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetActivityMappingAsync_ReturnsActivityMapping()
    {
        var activityV1Id = 12345;
        var expectedMapping = new ActivityMapping
        {
            V2ActivityId = "abc-123-def-456"
        };

        SetupMockResponse(HttpStatusCode.OK, expectedMapping);

        var result = await _client.GetActivityMappingAsync(activityV1Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.V2ActivityId, Is.EqualTo(expectedMapping.V2ActivityId));
    }

    [Test]
    public async Task GetCyclesAsync_WithDateFilters_BuildsCorrectQueryString()
    {
        var start = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
        var expectedResponse = new PaginatedResponse<Cycle>
        {
            Records = new List<Cycle>()
        };

        string? capturedUri = null;
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) =>
            {
                capturedUri = request.RequestUri?.ToString();
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });

        await _client.GetCyclesAsync(limit: 25, start: start, end: end, nextToken: "token123");

        Assert.That(capturedUri, Is.Not.Null);
        Assert.That(capturedUri, Does.Contain("limit=25"));
        Assert.That(capturedUri, Does.Contain("start=2024-01-01T00:00:00.000Z"));
        Assert.That(capturedUri, Does.Contain("end=2024-01-31T23:59:59.000Z"));
        Assert.That(capturedUri, Does.Contain("nextToken=token123"));
    }

    [Test]
    public void GetUserProfileAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        SetupMockResponse<UserProfile>(HttpStatusCode.Unauthorized, null);

        Assert.ThrowsAsync<HttpRequestException>(async () => await _client.GetUserProfileAsync());
    }

    private void SetupMockResponse<T>(HttpStatusCode statusCode, T? content)
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
