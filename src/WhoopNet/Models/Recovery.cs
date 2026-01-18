using System.Text.Json.Serialization;

namespace WhoopNet.Models;

/// <summary>
/// Represents a user's recovery metrics.
/// </summary>
public sealed class Recovery
{
    /// <summary>
    /// The unique identifier for the recovery.
    /// </summary>
    [JsonPropertyName("cycle_id")]
    public int CycleId { get; init; }

    /// <summary>
    /// The sleep ID associated with this recovery.
    /// </summary>
    [JsonPropertyName("sleep_id")]
    public int SleepId { get; init; }

    /// <summary>
    /// The user ID associated with this recovery.
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; init; }

    /// <summary>
    /// The timestamp when recovery was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// The timestamp when recovery was last updated.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// The recovery score information.
    /// </summary>
    [JsonPropertyName("score")]
    public RecoveryScore? Score { get; init; }
}
