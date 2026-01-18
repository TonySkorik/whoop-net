using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a physiological cycle.
/// </summary>
public sealed class Cycle
{
    /// <summary>
    /// The unique identifier for the cycle.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>
    /// The user ID associated with this cycle.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; init; }

    /// <summary>
    /// The start time of the cycle in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; init; }

    /// <summary>
    /// The end time of the cycle in ISO 8601 format.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; init; }

    /// <summary>
    /// The timezone offset from UTC.
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; init; }

    /// <summary>
    /// The score for this cycle.
    /// </summary>
    [JsonPropertyName("score")]
    public CycleScore? Score { get; init; }
}
