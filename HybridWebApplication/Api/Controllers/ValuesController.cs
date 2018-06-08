using Api.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers {
	public class ValuesController : ApiController {
		/// <summary>
		/// GET api/values
		/// Use default Authorize to validate scopes set on Startup.
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public string Get() {
			return "This is a resource";
		}

		/// <summary>
		/// If you wish to validate additional scopes use ClaimsAuthorize with a 
		/// variable number of Claims as parameters
		/// </summary>
		/// <returns></returns>
		[Route("api/values/list")]
		[ClaimsAuthorize("sample-api-list")]
		[HttpGet]
		public string[] List() {
			return new string[] { "This is first resource", "This is second resource" };
		}

	}
}
