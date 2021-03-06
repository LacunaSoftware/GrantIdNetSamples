﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Api {
	public static class WebApiConfig {
		public static HttpConfiguration Register() {
			var config = new HttpConfiguration();
			config.Formatters.Remove(config.Formatters.XmlFormatter);
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional }
			);
			return config;
		}
	}
}
