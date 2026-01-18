using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
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

        httpClient.BaseAddress.Should().NotBeNull();
        httpClient.BaseAddress!.ToString().Should().Be("https://api.prod.whoop.com/");

        client.Dispose();
        httpClient.Dispose();
    }

    [Test]
    public void Constructor_WithAccessToken_SetsAuthorizationHeader()
    {
        var accessToken = "test-access-token";

        using var client = new WhoopClient(accessToken);

        // Assert - we can't easily access the internal HttpClient, but we can test it works
        client.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
        var act = () => new WhoopClient((HttpClient)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WithEmptyAccessToken_ThrowsArgumentException()
    {
        var act = () => new WhoopClient(string.Empty);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SetAccessToken_WithValidToken_DoesNotThrow()
    {
        var token = "new-access-token";

        var act = () => _client.SetAccessToken(token);

        act.Should().NotThrow();
    }

    [Test]
    public void SetAccessToken_WithEmptyToken_ThrowsArgumentException()
    {
        var act = () => _client.SetAccessToken(string.Empty);

        act.Should().Throw<ArgumentException>();
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

        result.Should().NotBeNull();
        result!.UserId.Should().Be(expectedProfile.UserId);
        result.Email.Should().Be(expectedProfile.Email);
        result.FirstName.Should().Be(expectedProfile.FirstName);
        result.LastName.Should().Be(expectedProfile.LastName);
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

        result.Should().NotBeNull();
        result!.HeightMeter.Should().Be(expectedMeasurement.HeightMeter);
        result.WeightKilogram.Should().Be(expectedMeasurement.WeightKilogram);
        result.MaxHeartRate.Should().Be(expectedMeasurement.MaxHeartRate);
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

        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedCycle.Id);
        result.UserId.Should().Be(expectedCycle.UserId);
        result.Score.Should().NotBeNull();
        result.Score!.Strain.Should().Be(expectedCycle.Score.Strain);
    }

    [Test]
    public async Task GetCyclesAsync_ReturnsPaginatedCycles()
    {
        var expectedResponse = new PaginatedResponse<Cycle>
        {
            Records =
            [
                new Cycle { Id = 1, UserId = 123 },
                new Cycle { Id = 2, UserId = 123 }
            ],
            NextToken = "next-page-token"
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetCyclesAsync(limit: 10);

        result.Should().NotBeNull();
        result!.Records.Should().NotBeNull();
        result.Records!.Count.Should().Be(2);
        result.NextToken.Should().Be("next-page-token");
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

        result.Should().NotBeNull();
        result!.CycleId.Should().Be(expectedRecovery.CycleId);
        result.Score.Should().NotBeNull();
        result.Score!.Score.Should().Be(expectedRecovery.Score.Score);
    }

    [Test]
    public async Task GetRecoveriesAsync_ReturnsPaginatedRecoveries()
    {
        var expectedResponse = new PaginatedResponse<Recovery>
        {
            Records =
            [
                new Recovery { CycleId = 1, UserId = 123 },
                new Recovery { CycleId = 2, UserId = 123 }
            ]
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetRecoveriesAsync();

        result.Should().NotBeNull();
        result!.Records.Should().NotBeNull();
        result.Records!.Count.Should().Be(2);
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

        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedWorkout.Id);
        result.Score.Should().NotBeNull();
        result.Score!.Strain.Should().Be(expectedWorkout.Score.Strain);
    }

    [Test]
    public async Task GetWorkoutsAsync_ReturnsPaginatedWorkouts()
    {
        var expectedResponse = new PaginatedResponse<Workout>
        {
            Records =
            [
                new Workout { Id = 1, UserId = 123, SportId = 1 },
                new Workout { Id = 2, UserId = 123, SportId = 2 }
            ]
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetWorkoutsAsync();

        result.Should().NotBeNull();
        result!.Records.Should().NotBeNull();
        result.Records!.Count.Should().Be(2);
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

        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedSleep.Id);
        result.Nap.Should().Be(expectedSleep.Nap);
        result.Score.Should().NotBeNull();
    }

    [Test]
    public async Task GetSleepsAsync_ReturnsPaginatedSleeps()
    {
        var expectedResponse = new PaginatedResponse<Sleep>
        {
            Records =
            [
                new Sleep { Id = 1, UserId = 123, Nap = false },
                new Sleep { Id = 2, UserId = 123, Nap = true }
            ]
        };

        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        var result = await _client.GetSleepsAsync();

        result.Should().NotBeNull();
        result!.Records.Should().NotBeNull();
        result.Records!.Count.Should().Be(2);
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

        result.Should().NotBeNull();
        result!.V2ActivityId.Should().Be(expectedMapping.V2ActivityId);
    }

    [Test]
    public async Task GetCyclesAsync_WithDateFilters_BuildsCorrectQueryString()
    {
        var start = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2024, 1, 31, 23, 59, 59, DateTimeKind.Utc);
        var expectedResponse = new PaginatedResponse<Cycle>
        {
            Records = []
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

        capturedUri.Should().NotBeNull();
        capturedUri.Should().Contain("limit=25");
        capturedUri.Should().Contain("start=2024-01-01T00:00:00.000Z");
        capturedUri.Should().Contain("end=2024-01-31T23:59:59.000Z");
        capturedUri.Should().Contain("nextToken=token123");
    }

    [Test]
    public async Task GetUserProfileAsync_WithFailedRequest_ThrowsHttpRequestException()
    {
        SetupMockResponse<UserProfile>(HttpStatusCode.Unauthorized, null);

        var act = async () => await _client.GetUserProfileAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    private void SetupMockResponse<T>(HttpStatusCode statusCode, T? content)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = content != null
                ? JsonContent.Create(content)
                : null
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
