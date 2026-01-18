using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a physiological cycle.
/// </summary>
public class Cycle
{
    /// <summary>
    /// The unique identifier for the cycle.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this cycle.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the cycle in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the cycle in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC.
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// The score for this cycle.
    /// </summary>
    [JsonPropertyName("score")]
    public CycleScore? Score { get; set; }
}
