using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a physiological cycle
/// </summary>
public class Cycle
{
    /// <summary>
    /// The unique identifier for the cycle
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this cycle
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the cycle in ISO 8601 format
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the cycle in ISO 8601 format
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// The score for this cycle
    /// </summary>
    [JsonPropertyName("score")]
    public CycleScore? Score { get; set; }
}

/// <summary>
/// Represents the score for a cycle
/// </summary>
public class CycleScore
{
    /// <summary>
    /// The strain score (0-21)
    /// </summary>
    [JsonPropertyName("strain")]
    public double? Strain { get; set; }

    /// <summary>
    /// The kilojoules burned
    /// </summary>
    [JsonPropertyName("kilojoule")]
    public double? Kilojoule { get; set; }

    /// <summary>
    /// The average heart rate
    /// </summary>
    [JsonPropertyName("average_heart_rate")]
    public int? AverageHeartRate { get; set; }

    /// <summary>
    /// The max heart rate
    /// </summary>
    [JsonPropertyName("max_heart_rate")]
    public int? MaxHeartRate { get; set; }
}
