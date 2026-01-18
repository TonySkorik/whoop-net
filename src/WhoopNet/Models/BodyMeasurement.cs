using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a user's body measurements.
/// </summary>
public class BodyMeasurement
{
    /// <summary>
    /// The user's height in meters.
    /// </summary>
    [JsonPropertyName("height_meter")]
    public double? HeightMeter { get; set; }

    /// <summary>
    /// The user's weight in kilograms.
    /// </summary>
    [JsonPropertyName("weight_kilogram")]
    public double? WeightKilogram { get; set; }

    /// <summary>
    /// The user's maximum heart rate in beats per minute.
    /// </summary>
    [JsonPropertyName("max_heart_rate")]
    public int? MaxHeartRate { get; set; }
}
