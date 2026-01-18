using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents the sleep stage summary
/// </summary>
public class StageSummary
{
    /// <summary>
    /// Total time in bed in milliseconds
    /// </summary>
    [JsonPropertyName("total_in_bed_time_milli")]
    public long TotalInBedTimeMilli { get; set; }

    /// <summary>
    /// Total time awake in milliseconds
    /// </summary>
    [JsonPropertyName("total_awake_time_milli")]
    public long TotalAwakeTimeMilli { get; set; }

    /// <summary>
    /// Total time in light sleep in milliseconds
    /// </summary>
    [JsonPropertyName("total_light_sleep_time_milli")]
    public long TotalLightSleepTimeMilli { get; set; }

    /// <summary>
    /// Total time in slow wave sleep in milliseconds
    /// </summary>
    [JsonPropertyName("total_slow_wave_sleep_time_milli")]
    public long TotalSlowWaveSleepTimeMilli { get; set; }

    /// <summary>
    /// Total time in REM sleep in milliseconds
    /// </summary>
    [JsonPropertyName("total_rem_sleep_time_milli")]
    public long TotalRemSleepTimeMilli { get; set; }

    /// <summary>
    /// The number of sleep cycles
    /// </summary>
    [JsonPropertyName("sleep_cycle_count")]
    public int SleepCycleCount { get; set; }

    /// <summary>
    /// The number of disturbances
    /// </summary>
    [JsonPropertyName("disturbance_count")]
    public int DisturbanceCount { get; set; }
}
