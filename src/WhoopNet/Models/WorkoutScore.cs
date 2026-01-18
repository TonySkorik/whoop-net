using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents the score for a workout.
/// </summary>
public sealed class WorkoutScore
{
    /// <summary>
    /// The strain score for this workout (0-21)
    /// </summary>
    [JsonPropertyName("strain")]
    public double? Strain { get; init; }

    /// <summary>
    /// The average heart rate during the workout.
    /// </summary>
    [JsonPropertyName("average_heart_rate")]
    public int? AverageHeartRate { get; init; }

    /// <summary>
    /// The max heart rate during the workout.
    /// </summary>
    [JsonPropertyName("max_heart_rate")]
    public int? MaxHeartRate { get; init; }

    /// <summary>
    /// The kilojoules burned during the workout.
    /// </summary>
    [JsonPropertyName("kilojoule")]
    public double? Kilojoule { get; init; }

    /// <summary>
    /// The percentage spent in each heart rate zone.
    /// </summary>
    [JsonPropertyName("zone_duration")]
    public ZoneDuration? ZoneDuration { get; init; }
}
