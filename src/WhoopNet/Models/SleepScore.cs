using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents the score for a sleep.
/// </summary>
public sealed class SleepScore
{
    /// <summary>
    /// The stage summary for the sleep.
    /// </summary>
    [JsonPropertyName("stage_summary")]
    public StageSummary? StageSummary { get; init; }

    /// <summary>
    /// The sleep performance percentage.
    /// </summary>
    [JsonPropertyName("sleep_performance_percentage")]
    public double? SleepPerformancePercentage { get; init; }

    /// <summary>
    /// The sleep consistency percentage.
    /// </summary>
    [JsonPropertyName("sleep_consistency_percentage")]
    public double? SleepConsistencyPercentage { get; init; }

    /// <summary>
    /// The sleep efficiency percentage.
    /// </summary>
    [JsonPropertyName("sleep_efficiency_percentage")]
    public double? SleepEfficiencyPercentage { get; init; }
}
