using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a sleep activity
/// </summary>
public class Sleep
{
    /// <summary>
    /// The unique identifier for the sleep
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The user ID associated with this sleep
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// The start time of the sleep
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime? Start { get; set; }

    /// <summary>
    /// The end time of the sleep
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }

    /// <summary>
    /// The timezone offset from UTC
    /// </summary>
    [JsonPropertyName("timezone_offset")]
    public string? TimezoneOffset { get; set; }

    /// <summary>
    /// Indicates if this is a nap
    /// </summary>
    [JsonPropertyName("nap")]
    public bool Nap { get; set; }

    /// <summary>
    /// The score for this sleep
    /// </summary>
    [JsonPropertyName("score")]
    public SleepScore? Score { get; set; }
}

/// <summary>
/// Represents the score for a sleep
/// </summary>
public class SleepScore
{
    /// <summary>
    /// The stage summary for the sleep
    /// </summary>
    [JsonPropertyName("stage_summary")]
    public StageSummary? StageSummary { get; set; }

    /// <summary>
    /// The sleep performance percentage
    /// </summary>
    [JsonPropertyName("sleep_performance_percentage")]
    public double? SleepPerformancePercentage { get; set; }

    /// <summary>
    /// The sleep consistency percentage
    /// </summary>
    [JsonPropertyName("sleep_consistency_percentage")]
    public double? SleepConsistencyPercentage { get; set; }

    /// <summary>
    /// The sleep efficiency percentage
    /// </summary>
    [JsonPropertyName("sleep_efficiency_percentage")]
    public double? SleepEfficiencyPercentage { get; set; }
}

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
