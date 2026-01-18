using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a workout/activity
/// </summary>
public class Workout
{
    /// <summary>
    /// The unique identifier for the workout
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this workout
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the workout
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the workout
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// The sport ID for this workout
    /// </summary>
    [JsonPropertyName("sport_id")]
    public int SportId { get; set; }

    /// <summary>
    /// The score for this workout
    /// </summary>
    [JsonPropertyName("score")]
    public WorkoutScore? Score { get; set; }
}

/// <summary>
/// Represents the score for a workout
/// </summary>
public class WorkoutScore
{
    /// <summary>
    /// The strain score for this workout (0-21)
    /// </summary>
    [JsonPropertyName("strain")]
    public double? Strain { get; set; }

    /// <summary>
    /// The average heart rate during the workout
    /// </summary>
    [JsonPropertyName("average_heart_rate")]
    public int? AverageHeartRate { get; set; }

    /// <summary>
    /// The max heart rate during the workout
    /// </summary>
    [JsonPropertyName("max_heart_rate")]
    public int? MaxHeartRate { get; set; }

    /// <summary>
    /// The kilojoules burned during the workout
    /// </summary>
    [JsonPropertyName("kilojoule")]
    public double? Kilojoule { get; set; }

    /// <summary>
    /// The percentage spent in each heart rate zone
    /// </summary>
    [JsonPropertyName("zone_duration")]
    public ZoneDuration? ZoneDuration { get; set; }
}

/// <summary>
/// Represents time spent in heart rate zones
/// </summary>
public class ZoneDuration
{
    /// <summary>
    /// Milliseconds in zone 0
    /// </summary>
    [JsonPropertyName("zone_zero_milli")]
    public long ZoneZeroMilli { get; set; }

    /// <summary>
    /// Milliseconds in zone 1
    /// </summary>
    [JsonPropertyName("zone_one_milli")]
    public long ZoneOneMilli { get; set; }

    /// <summary>
    /// Milliseconds in zone 2
    /// </summary>
    [JsonPropertyName("zone_two_milli")]
    public long ZoneTwoMilli { get; set; }

    /// <summary>
    /// Milliseconds in zone 3
    /// </summary>
    [JsonPropertyName("zone_three_milli")]
    public long ZoneThreeMilli { get; set; }

    /// <summary>
    /// Milliseconds in zone 4
    /// </summary>
    [JsonPropertyName("zone_four_milli")]
    public long ZoneFourMilli { get; set; }

    /// <summary>
    /// Milliseconds in zone 5
    /// </summary>
    [JsonPropertyName("zone_five_milli")]
    public long ZoneFiveMilli { get; set; }
}
