using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Claims;
using System.Web;

namespace Api.Attributes {
	public class ClaimsAuthorize : AuthorizeAttribute {
		private List<string> requiredScopes { get; set; }

		public ClaimsAuthorize(params string[] scopes) {
			if (scopes != null && scopes.Length > 0) {
				requiredScopes = scopes.ToList();
			} else {
				requiredScopes = new List<string>();
			}
		}

		protected override bool IsAuthorized(HttpActionContext actionContext) {
			ClaimsIdentity claimsIdentity;
			var httpContext = HttpContext.Current;
			if (!(httpContext.User.Identity is ClaimsIdentity)) {
				return false;
			}

			claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

			var allRequiredScopes = new List<string>(requiredScopes);

			var claimScopes = claimsIdentity.Claims.Where(c => c.Type == "scope").Select(c => c.Value);
			var allScopes = new List<string>();

			foreach (var scope in claimScopes) {
				var multiScope = scope.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				allScopes.AddRange(multiScope.ToList());
			}

			if (requiredScopes.Intersect(allScopes).Count() != requiredScopes.Count) {
				return false;
			}

			//Continue with the regular Authorize check
			return base.IsAuthorized(actionContext);
		}
	}
}