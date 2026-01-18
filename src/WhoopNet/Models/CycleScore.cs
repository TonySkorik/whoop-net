using System.Text.Json.Serialization;

namespace WhoopNet.Models;

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
