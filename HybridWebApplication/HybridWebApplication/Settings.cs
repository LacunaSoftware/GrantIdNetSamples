using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HybridWebApplication {
	public class Settings {
		public static string AppId {
			get {
				return ConfigurationManager.AppSettings["AppId"];
			}
		}
		public static string ApplicationSecret {
			get {
				return ConfigurationManager.AppSettings["ApplicationSecret"];
			}
		}
		public static string RedirectUri {
			get {
				return ConfigurationManager.AppSettings["RedirectUri"];
			}
		}
		public static string SiteUrl {
			get {
				return ConfigurationManager.AppSettings["SiteUrl"];
			}
		}
		public static string GrantIdSubscriptionUrl {
			get {
				return ConfigurationManager.AppSettings["GrantIdSubscriptionUrl"];
			}
		}
		public static string PrivateResourceRoute {
			get {
				return ConfigurationManager.AppSettings["PrivateResourceRoute"];
			}
		}

		public static string ApiScopes {
			get {
				return ConfigurationManager.AppSettings["ApiScopes"];
			}
		}
	}
}