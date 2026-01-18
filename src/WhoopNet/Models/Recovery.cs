using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a user's recovery metrics
/// </summary>
public class Recovery
{
    /// <summary>
    /// The unique identifier for the recovery
    /// </summary>
    [JsonPropertyName("cycle_id")]
    public int CycleId { get; set; }

    /// <summary>
    /// The sleep ID associated with this recovery
    /// </summary>
    [JsonPropertyName("sleep_id")]
    public int SleepId { get; set; }

    /// <summary>
    /// The user ID associated with this recovery
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The timestamp when recovery was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// The timestamp when recovery was last updated
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// The recovery score information
    /// </summary>
    [JsonPropertyName("score")]
    public RecoveryScore? Score { get; set; }
}

/// <summary>
/// Represents the recovery score information
/// </summary>
public class RecoveryScore
{
    /// <summary>
    /// The user's recovery percentage (0-100)
    /// </summary>
    [JsonPropertyName("user_calibrating")]
    public bool UserCalibrating { get; set; }

    /// <summary>
    /// The recovery score (0-100)
    /// </summary>
    [JsonPropertyName("recovery_score")]
    public double? Score { get; set; }

    /// <summary>
    /// The resting heart rate
    /// </summary>
    [JsonPropertyName("resting_heart_rate")]
    public int? RestingHeartRate { get; set; }

    /// <summary>
    /// The heart rate variability in milliseconds
    /// </summary>
    [JsonPropertyName("hrv_rmssd_milli")]
    public double? HrvRmssdMilli { get; set; }

    /// <summary>
    /// The SpO2 percentage
    /// </summary>
    [JsonPropertyName("spo2_percentage")]
    public double? Spo2Percentage { get; set; }

    /// <summary>
    /// The skin temperature in Celsius
    /// </summary>
    [JsonPropertyName("skin_temp_celsius")]
    public double? SkinTempCelsius { get; set; }
}
