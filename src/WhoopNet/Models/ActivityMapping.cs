using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents the response from the activity mapping endpoint
/// </summary>
public class ActivityMapping
{
    /// <summary>
    /// The v2 UUID for the activity
    /// </summary>
    [JsonPropertyName("v2_activity_id")]
    public string? V2ActivityId { get; set; }
}
