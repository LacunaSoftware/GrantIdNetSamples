using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLoginConsoleApplication {
	public class Settings {
		public static string GrantIdUrl {
			get {
				return ConfigurationManager.AppSettings["GrantIdUrl"];
			}
		}

		public static string AppId {
			get {
				return ConfigurationManager.AppSettings["AppId"];
			}
		}
		public static string AppSecret {
			get {
				return ConfigurationManager.AppSettings["AppSecret"];
			}
		}

		public static string AppIdCertificate {
			get {
				return ConfigurationManager.AppSettings["AppIdCertificate"];
			}
		}
		public static string AppSecretCertificate {
			get {
				return ConfigurationManager.AppSettings["AppSecretCertificate"];
			}
		}
	}
}
