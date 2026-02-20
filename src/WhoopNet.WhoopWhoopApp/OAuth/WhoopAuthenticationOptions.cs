/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.OAuth;

namespace WhoopNet.WhoopWhoopApp.OAuth;

/// <summary>
/// Defines a set of options used by <see cref="WhoopAuthenticationHandler"/>.
/// </summary>
public class WhoopAuthenticationOptions : OAuthOptions
{
	public WhoopAuthenticationOptions()
	{
		ClaimsIssuer = WhoopAuthenticationDefaults.Issuer;
		CallbackPath = WhoopAuthenticationDefaults.CallbackPath;

		AuthorizationEndpoint = WhoopAuthenticationDefaults.AuthorizationEndpoint;
		TokenEndpoint = WhoopAuthenticationDefaults.TokenEndpoint;
		UserInformationEndpoint = WhoopAuthenticationDefaults.UserInformationEndpoint;

		Scope.Add("auth_user");

		//ClaimActions.MapJsonKey(Claims.Avatar, "avatar");
		//ClaimActions.MapJsonKey(Claims.City, "city");
		//ClaimActions.MapJsonKey(Claims.Gender, "gender");
		//ClaimActions.MapJsonKey(Claims.Nickname, "nick_name");
		//ClaimActions.MapJsonKey(Claims.Province, "province");
	}
}
