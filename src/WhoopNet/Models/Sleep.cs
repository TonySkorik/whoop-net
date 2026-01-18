using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a sleep activity.
/// </summary>
public class Sleep
{
    /// <summary>
    /// The unique identifier for the sleep.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this sleep.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the sleep.
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the sleep.
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC.
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// Indicates if this is a nap.
    /// </summary>
    [JsonPropertyName("nap")]
    public bool Nap { get; set; }

    /// <summary>
    /// The score for this sleep.
    /// </summary>
    [JsonPropertyName("score")]
    public SleepScore? Score { get; set; }
}
