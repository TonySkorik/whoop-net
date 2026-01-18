using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a workout/activity.
/// </summary>
public class Workout
{
    /// <summary>
    /// The unique identifier for the workout.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this workout.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the workout.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the workout.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC.
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// The sport ID for this workout.
    /// </summary>
    [JsonPropertyName("sport_id")]
    public int SportId { get; set; }

    /// <summary>
    /// The score for this workout.
    /// </summary>
    [JsonPropertyName("score")]
    public WorkoutScore? Score { get; set; }
}
