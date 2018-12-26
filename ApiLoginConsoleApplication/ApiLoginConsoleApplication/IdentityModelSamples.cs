using Grant.IdentityModel;
using Grant.IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLoginConsoleApplication {
	public class IdentityModelSamples {
		private static TokenClient _tokenClient;
		private static TokenClient TokenClient {
			get {
				if (_tokenClient == null) {
					_tokenClient = GetTokenClient(Settings.AppId, Settings.AppSecret);
				}

				return _tokenClient;
			}
		}

		private static TokenClient GetTokenClient(string appId, string secret) {
			var tokenUrl = new Uri(new Uri(Settings.GrantIdUrl), "/connect/token").ToString();
			return new TokenClient(tokenUrl, appId, secret);
		}

		/// <summary>
		/// Regular login, using the username that is configured in the Subscription/Application
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static async Task<string> Login(string username, string password) {
			var client = GetTokenClient(Settings.AppId, Settings.AppSecret);

			var response = await client.RequestResourceOwnerPasswordAsync(username, password, scope: "openid profile");//you may add more scopes if you need more information/acess to APIs

			await DisplayResponse(response);

			return response.AccessToken;
		}

		private static async Task DisplayResponse(TokenResponse response) {
			if (response.IsError) {
				Console.WriteLine($"Error during login: {response.Error} - {response.ErrorDescription}");
				return;
			}

			Console.WriteLine("User logged in successfully!");

			//This is the user's access token, use it to call APIs OR
			Console.WriteLine($"Access token:\r\n{response.AccessToken}");

			//to call the User Info endpoint to retrieve more information about the user:
			var userInfoUrl = new Uri(new Uri(Settings.GrantIdUrl), "/connect/userinfo").ToString();
			var userInfoClient = new UserInfoClient(userInfoUrl);

			var userInfoResponse = await userInfoClient.GetAsync(response.AccessToken);

			//The "sub" claim contains the user's Id
			var userId = userInfoResponse.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject).Value;
			Console.WriteLine($"\r\nId: {userId}");

			//Other claims
			userInfoResponse.Claims.Where(c => c.Type != JwtClaimTypes.Subject).ToList().ForEach(c => Console.WriteLine($"{c.Type}: {c.Value}"));
		}
	}
}
