using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents time spent in heart rate zones.
/// </summary>
public sealed class ZoneDuration
{
    /// <summary>
    /// Milliseconds in zone 0.
    /// </summary>
    [JsonPropertyName("zone_zero_milli")]
    public long ZoneZeroMilli { get; init; }

    /// <summary>
    /// Milliseconds in zone 1.
    /// </summary>
    [JsonPropertyName("zone_one_milli")]
    public long ZoneOneMilli { get; init; }

    /// <summary>
    /// Milliseconds in zone 2.
    /// </summary>
    [JsonPropertyName("zone_two_milli")]
    public long ZoneTwoMilli { get; init; }

    /// <summary>
    /// Milliseconds in zone 3.
    /// </summary>
    [JsonPropertyName("zone_three_milli")]
    public long ZoneThreeMilli { get; init; }

    /// <summary>
    /// Milliseconds in zone 4.
    /// </summary>
    [JsonPropertyName("zone_four_milli")]
    public long ZoneFourMilli { get; init; }

    /// <summary>
    /// Milliseconds in zone 5.
    /// </summary>
    [JsonPropertyName("zone_five_milli")]
    public long ZoneFiveMilli { get; init; }
}
