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

			app.UseIdentityServerBearerTokenAuthentication(new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions() {
				Authority = ConfigurationManager.AppSettings["GrantIdSubscriptionUrl"]
			});

			app.UseWebApi(WebApiConfig.Register());
		}

	}
}