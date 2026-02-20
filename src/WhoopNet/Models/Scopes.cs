namespace WhoopNet.Models;

/// <summary>
/// Represents available scopes for OAuth.
/// </summary>
public class Scopes
{
	public const string AllScopes = "read:profile read:recovery read:cycles read:sleep read:workout read:body_measurement";

	public const string Profile = "read:profile";

	public const string Recovery = "read:recovery";

	public const string Cycles = "read:cycles";

	public const string Sleep = "read:sleep";

	public const string Workout = "read:workout";

	public const string BodyMeasurement = "read:body_measurement";
}
