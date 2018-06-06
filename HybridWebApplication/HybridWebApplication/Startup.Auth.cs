using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HybridWebApplication {
	public partial class Startup {

		public static string PublicClientId { get; private set; }

		public void ConfigureAuth(IAppBuilder app) {

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseCookieAuthentication(new CookieAuthenticationOptions() {
				CookieName = ".AspNet.ApplicationCookie.HybridWebApp"
			});

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Configure the application for OAuth based flow
			PublicClientId = "self";
		}
	}
}