/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Whoop;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace WhoopNet.WhoopWhoopApp.OAuth;

/// <summary>
/// Extension methods to add Whoop authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class WhoopAuthenticationExtensions
{
	/// <summary>
	/// Adds <see cref="WhoopAuthenticationHandler"/> to the specified
	/// <see cref="AuthenticationBuilder"/>, which enables Whoop authentication capabilities.
	/// </summary>
	/// <param name="builder">The authentication builder.</param>
	/// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
	public static AuthenticationBuilder AddWhoop([NotNull] this AuthenticationBuilder builder)
	{
		return builder.AddWhoop(WhoopAuthenticationDefaults.AuthenticationScheme, options => { });
	}

	/// <summary>
	/// Adds <see cref="WhoopAuthenticationHandler"/> to the specified
	/// <see cref="AuthenticationBuilder"/>, which enables Whoop authentication capabilities.
	/// </summary>
	/// <param name="builder">The authentication builder.</param>
	/// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
	/// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
	public static AuthenticationBuilder AddWhoop(
		[NotNull] this AuthenticationBuilder builder,
		[NotNull] Action<WhoopAuthenticationOptions> configuration)
	{
		return builder.AddWhoop(WhoopAuthenticationDefaults.AuthenticationScheme, configuration);
	}

	/// <summary>
	/// Adds <see cref="WhoopAuthenticationHandler"/> to the specified
	/// <see cref="AuthenticationBuilder"/>, which enables Whoop authentication capabilities.
	/// </summary>
	/// <param name="builder">The authentication builder.</param>
	/// <param name="scheme">The authentication scheme associated with this instance.</param>
	/// <param name="configuration">The delegate used to configure the Whoop options.</param>
	/// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
	public static AuthenticationBuilder AddWhoop(
		[NotNull] this AuthenticationBuilder builder,
		[NotNull] string scheme,
		[NotNull] Action<WhoopAuthenticationOptions> configuration)
	{
		return builder.AddWhoop(scheme, WhoopAuthenticationDefaults.DisplayName, configuration);
	}

	/// <summary>
	/// Adds <see cref="WhoopAuthenticationHandler"/> to the specified
	/// <see cref="AuthenticationBuilder"/>, which enables Whoop authentication capabilities.
	/// </summary>
	/// <param name="builder">The authentication builder.</param>
	/// <param name="scheme">The authentication scheme associated with this instance.</param>
	/// <param name="caption">The optional display name associated with this instance.</param>
	/// <param name="configuration">The delegate used to configure the Whoop options.</param>
	/// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
	public static AuthenticationBuilder AddWhoop(
		[NotNull] this AuthenticationBuilder builder,
		[NotNull] string scheme,
		string caption,
		[NotNull] Action<WhoopAuthenticationOptions> configuration)
	{
		return builder.AddOAuth<WhoopAuthenticationOptions, WhoopAuthenticationHandler>(scheme, caption, configuration);
	}
}
