using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Glader.ASP.Vivox
{
	public sealed class DefaultLocalVivoxTokenSigningService : IVivoxTokenSignService
	{
		private IVivoxAPIKeyRepository APIKeyProvider { get; }

		public DefaultLocalVivoxTokenSigningService(IVivoxAPIKeyRepository apiKeyProvider)
		{
			APIKeyProvider = apiKeyProvider ?? throw new ArgumentNullException(nameof(apiKeyProvider));
		}

		/// <inheritdoc />
		public async Task<string> CreateSignatureAsync(VivoxTokenClaims claims, CancellationToken token = default)
		{
			if (claims == null) throw new ArgumentNullException(nameof(claims));

			string claimsString = JsonConvert.SerializeObject(claims);

			//Base64URLEncoding from: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/master/src/Microsoft.IdentityModel.Tokens/Base64UrlEncoder.cs
			claimsString = Base64UrlEncoder.Encode(claimsString);

			//e30 is {} header
			string signable = $"e30.{claimsString}";

			return $"{signable}.{SHA256Hash(await APIKeyProvider.RetrieveKey(token), signable)}";
		}

		private static string SHA256Hash(string secret, string message)
		{
			if (string.IsNullOrWhiteSpace(secret)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(secret));
			if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

			//TODO: Optimize
			byte[] keyByte = System.Text.Encoding.ASCII.GetBytes(secret);
			byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(message);

			using var hmacsha256 = new HMACSHA256(keyByte);
			byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
			return Base64UrlEncoder.Encode(hashmessage);
		}
	}
}
