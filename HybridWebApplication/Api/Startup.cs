using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Api.Startup))]

namespace Api {
	public class Startup {

		public void Configuration(IAppBuilder app) {
			System.IdentityModel.Tokens.JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

			var requiredScopes = new List<string>();
			var apiScope = ConfigurationManager.AppSettings["ApiScope"];

			if (!string.IsNullOrEmpty(apiScope)) {
				requiredScopes.Add(apiScope);
			}

			app.UseIdentityServerBearerTokenAuthentication(new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions() {
				Authority = ConfigurationManager.AppSettings["GrantIdSubscriptionUrl"],
				RequiredScopes = requiredScopes
			});

			app.UseWebApi(WebApiConfig.Register());
		}

	}
}