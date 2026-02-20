/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace WhoopNet.WhoopWhoopApp.OAuth;

/// <summary>
/// Default values for Whoop authentication.
/// </summary>
public static class WhoopAuthenticationDefaults
{
	/// <summary>
	/// Default value for <see cref="AuthenticationScheme.Name"/>.
	/// </summary>
	public const string AuthenticationScheme = "Whoop";

	/// <summary>
	/// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
	/// </summary>
	public static readonly string DisplayName = "Whoop";

	/// <summary>
	/// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
	/// </summary>
	public static readonly string Issuer = "Whoop";

	/// <summary>
	/// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
	/// </summary>
	public static readonly string CallbackPath = "/signin-whoop";

	/// <summary>
	/// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
	/// </summary>
	public static readonly string AuthorizationEndpoint = "https://api.prod.whoop.com/oauth/oauth2/auth";

	/// <summary>
	/// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
	/// </summary>
	public static readonly string TokenEndpoint = "https://api.prod.whoop.com/oauth/oauth2/token";

	/// <summary>
	/// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
	/// </summary>
	public static readonly string UserInformationEndpoint = "https://api.prod.whoop.com/v2/user/profile/basic";
}
