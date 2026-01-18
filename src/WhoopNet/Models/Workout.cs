using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a workout/activity.
/// </summary>
public sealed class Workout
{
    /// <summary>
    /// The unique identifier for the workout.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>
    /// The user ID associated with this workout.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; init; }

    /// <summary>
    /// The start time of the workout.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; init; }

    /// <summary>
    /// The end time of the workout.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; init; }

    /// <summary>
    /// The timezone offset from UTC.
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; init; }

    /// <summary>
    /// The sport ID for this workout.
    /// </summary>
    [JsonPropertyName("sport_id")]
    public int SportId { get; init; }

    /// <summary>
    /// The score for this workout.
    /// </summary>
    [JsonPropertyName("score")]
    public WorkoutScore? Score { get; init; }
}
