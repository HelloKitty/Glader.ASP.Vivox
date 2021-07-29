using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// Contract for service that signs a Vivox claims.
	/// </summary>
	public interface IVivoxTokenSignService
	{
		/// <summary>
		/// Creates a signed token authorizing it as officially
		/// useable.
		/// </summary>
		/// <param name="claims">The token claims</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>The signed authorization token in string form.</returns>
		Task<string> CreateSignatureAsync(VivoxTokenClaims claims, CancellationToken token = default);
	}
}
