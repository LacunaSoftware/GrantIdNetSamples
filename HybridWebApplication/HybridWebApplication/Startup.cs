using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(HybridWebApplication.Startup))]

namespace HybridWebApplication {
	public partial class Startup {

		public void Configuration(IAppBuilder app) {

			app.UseCookieAuthentication(new CookieAuthenticationOptions() {
				AuthenticationType = "Cookies"
			});

			var apiScopes = string.Empty;

			if (!string.IsNullOrEmpty(Settings.ApiScopes)) {
				apiScopes = " " + Settings.ApiScopes;
			}

			//Hybrid type of application
			app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions() {
				Authority = Settings.GrantIdSubscriptionUrl,

				ClientId = Settings.AppId,
				ClientSecret = Settings.ApplicationSecret,
				Scope = "openid profile" + apiScopes,
				ResponseType = "code id_token",
				RedirectUri = Settings.RedirectUri,
				PostLogoutRedirectUri = Settings.RedirectUri,
				SignInAsAuthenticationType = "Cookies",
				UseTokenLifetime = true,

				Notifications = new OpenIdConnectAuthenticationNotifications {
					AuthorizationCodeReceived = async n => {
						// use the code to get the access and refresh token
						var tokenClient = new TokenClient(new Uri(n.Options.Authority + "/connect/token").ToString(), n.Options.ClientId, n.Options.ClientSecret);

						var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);

						if (tokenResponse.IsError) {
							throw new Exception(tokenResponse.Error);
						}

						var id = n.AuthenticationTicket.Identity;
						id.AddClaim(new Claim("access_token", tokenResponse.AccessToken));

						id.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
						
						id.AddClaims(n.JwtSecurityToken.Claims);

						n.AuthenticationTicket = new AuthenticationTicket(
							new ClaimsIdentity(id.Claims,
							n.AuthenticationTicket.Identity.AuthenticationType, "name", "role"),
							n.AuthenticationTicket.Properties);

						//Get User ID and register it in local Database if necessary
						var userIdClaim = id.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
						var userId = userIdClaim?.Value;						
					},
					RedirectToIdentityProvider = n => {
						n.ProtocolMessage.Parameters.Add("culture", "pt-BR");
						//when logging out the id token must be sent
						if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout) {
							var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

							if (idTokenHint != null) {
								n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
							}
						}

						return Task.FromResult(0);
					}
				}
			});

		}

	}
}