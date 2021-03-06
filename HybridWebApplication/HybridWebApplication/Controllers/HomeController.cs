﻿using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HybridWebApplication.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {
			return View();
		}

		public ActionResult Error() {
			return View();
		}

		public ActionResult Logout() {
			if (User.Identity.IsAuthenticated) {
				Request.GetOwinContext().Authentication.SignOut();
			}
			return Redirect("/");
		}

		[Authorize]
		public ActionResult PopupCallback() {
			return View();
		}


		[Authorize]
		public async Task<ActionResult> PrivateRoute(bool refresh = false) {
			if (refresh) {
				var cookie = Request.Cookies[".AspNet.Cookies"];
				if (cookie != null) {
					cookie.Expires = DateTime.Now.AddYears(-1);
					Response.Cookies.Add(cookie);
				}

				return RedirectToAction(nameof(PrivateRoute));
			}

			var principal = (ClaimsPrincipal)User;
			ViewBag.Name = principal.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
			ViewBag.Phone = principal.Claims.FirstOrDefault(c => c.Type == "phone")?.Value;
			ViewBag.Email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

			return View();
		}

		private HttpClient getApiClient() {
			var principal = (ClaimsPrincipal)User;
			var accessToken = principal.Claims.FirstOrDefault(c => c.Type == "access_token").Value;

			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			return httpClient;
		}

		[Authorize]
		public async Task<ActionResult> PrivateResource() {
			var httpClient = getApiClient();
			var response = await httpClient.GetAsync(Settings.PrivateResourceRoute);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode) {
				ViewBag.PrivateResource = json;
			} else {
				ViewBag.Error = response.StatusCode.ToString() + (!string.IsNullOrEmpty(json) ? ":" + json : string.Empty);
			}

			return View();
		}

		[Authorize]
		public async Task<ActionResult> PrivateResource2() {
			var httpClient = getApiClient();

			var response = await httpClient.GetAsync(Settings.PrivateResourceRoute + "/list");
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode) {
				ViewBag.PrivateResource = json;
			} else {
				ViewBag.Error = response.StatusCode.ToString() + (!string.IsNullOrEmpty(json) ? ":" + json : string.Empty);
			}

			return View();
		}
	}
}