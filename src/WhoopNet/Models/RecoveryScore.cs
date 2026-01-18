using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents the recovery score information.
/// </summary>
public sealed class RecoveryScore
{
    /// <summary>
    /// Indicates whether the user is in calibration mode.
    /// </summary>
    [JsonPropertyName("user_calibrating")]
    public bool UserCalibrating { get; init; }

    /// <summary>
    /// The recovery score (0-100)
    /// </summary>
    [JsonPropertyName("recovery_score")]
    public double? Score { get; init; }

    /// <summary>
    /// The resting heart rate.
    /// </summary>
    [JsonPropertyName("resting_heart_rate")]
    public int? RestingHeartRate { get; init; }

    /// <summary>
    /// The heart rate variability in milliseconds.
    /// </summary>
    [JsonPropertyName("hrv_rmssd_milli")]
    public double? HrvRmssdMilli { get; init; }

    /// <summary>
    /// The SpO2 percentage.
    /// </summary>
    [JsonPropertyName("spo2_percentage")]
    public double? Spo2Percentage { get; init; }

    /// <summary>
    /// The skin temperature in Celsius.
    /// </summary>
    [JsonPropertyName("skin_temp_celsius")]
    public double? SkinTempCelsius { get; init; }
}
