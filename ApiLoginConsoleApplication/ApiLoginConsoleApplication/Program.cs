using Grant.Id.Client;
using Grant.IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLoginConsoleApplication {
	public class Program {
		private static GrantIdUserService _userService;

		//Requires an Application registered in GrantID with type Resource Owner Password
		//and allowed Identification Scopes: User Identifier and User Profile
		private static GrantIdUserService UserService {
			get {
				if (_userService == null) {
					_userService = new GrantIdUserService(new GrantIdSubscription(new GrantIdOptions() {
						AuthServerUrl = Settings.GrantIdUrl,
						ResourceOwnerPasswordAppId = Settings.AppId,
						ResourceOwnerPasswordAppSecret = Settings.AppSecret
					}));
				}

				return _userService;
			}
		}

		private static GrantIdUserService _userServiceCertificate;

		//Requires an Application registered in GrantID with type Resource Owner Password,
		//allowed Identification Scopes: User Identifier, User Profile, CPF,
		//Required Claims set to CPF,
		//Login Identifier set to CPF 
		//and login with digital certificate enabled
		private static GrantIdUserService UserServiceCertificate {
			get {
				if (_userServiceCertificate == null) {
					_userServiceCertificate = new GrantIdUserService(new GrantIdSubscription(new GrantIdOptions() {
						AuthServerUrl = Settings.GrantIdUrl,
						ResourceOwnerPasswordAppId = Settings.AppIdCertificate,
						ResourceOwnerPasswordAppSecret = Settings.AppSecretCertificate
					}));
				}

				return _userServiceCertificate;
			}
		}

		static void Main(string[] args) {
			MainAsync(args).GetAwaiter().GetResult();
		}

		static async Task MainAsync(string[] args) {
			await Login("testskywalker@lacunasoftware.com", "Password*123");

			//Login with certificate
			var certificateB64 = "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";
			await LoginWithCertificate(certificateB64);
		}

		/// <summary>
		/// Regular login, using the username that is configured in the Subscription/Application
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		static async Task<string> Login(string username, string password) {
			var result = await UserService.ResourceOwnerPasswordSignInAsync(username, password, new List<string>() { "openid", "profile" }); //you may add more scopes if you need more information/acess to APIs

			await DisplayResult(result);

			return result.AccessToken;
		}

		/// <summary>
		/// Login with digital certificate. The Application must be configured to require digital certificate login.
		/// </summary>
		/// <param name="cpf">The "CPF" identifier</param>
		/// <param name="password"></param>
		/// <returns></returns>
		static async Task LoginWithCertificate(string certificatePFXB64) {

			//1- Get a challenge
			var challenge = await UserServiceCertificate.GetChallenge();

			//2- Sign the challenge
			var signatureResult = SignatureUtil.SignDataAndGetCertificate(certificatePFXB64, challenge);

			//3- Login
			var result = await UserServiceCertificate.ResourceOwnerCertificateSignInAsync(challenge, signatureResult.Certificate, signatureResult.Signature,
				new List<string>() { "openid", "profile", "cpf" }); //you may add more scopes if you need more information/acess to APIs

			await DisplayResult(result);
		}

		private static async Task DisplayResult(SignInResult result) {
			if (!result.Succeeded) {
				Console.WriteLine($"\r\nError during login: {result.FailReason.ToString()}");
				return;
			}

			Console.WriteLine("\r\nUser logged in successfully!");

			//This is the user's access token, use it to call APIs OR
			Console.WriteLine($"Access token:\r\n{result.AccessToken}");

			var userInfo = await UserService.GetUserInfoAsync(result.AccessToken);

			//The "sub" claim contains the user's Id
			var userId = userInfo.Claims.FirstOrDefault(c => c.Name == JwtClaimTypes.Subject).Value;
			Console.WriteLine($"\r\nId: {userId}");

			//Other claims
			userInfo.Claims.Where(c => c.Name != JwtClaimTypes.Subject).ToList().ForEach(c => Console.WriteLine($"{c.Name}: {c.Value}"));
		}
	}
}
