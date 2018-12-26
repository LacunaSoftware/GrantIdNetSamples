using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApiLoginConsoleApplication {
	public class SignatureUtil {
		public static SignatureResult SignDataAndGetCertificate(string certificatePFXB64, byte[] data) {
			var certificate = new X509Certificate2(Convert.FromBase64String(certificatePFXB64));
			var cert = certificate.Export(X509ContentType.Cert);
			var key = (RSACryptoServiceProvider)certificate.PrivateKey;

			var enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
			var cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, key.CspKeyContainerInfo.KeyContainerName);
			var privateKey = new RSACryptoServiceProvider(cspparams);

			var signature = privateKey.SignData(data, CryptoConfig.MapNameToOID("SHA256"));

			return new SignatureResult() {
				Signature = signature, Certificate = cert
			};
		}
	}

	public class SignatureResult {
		public byte[] Signature { get; set; }
		public byte[] Certificate { get; set; }
	}
}
