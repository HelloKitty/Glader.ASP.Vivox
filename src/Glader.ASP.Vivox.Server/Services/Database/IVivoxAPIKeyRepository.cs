using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// Contract for providers that provide an API key for Vivox.
	/// </summary>
	public interface IVivoxAPIKeyRepository
	{
		/// <summary>
		/// Retrieves the Vivox API key from the store.
		/// </summary>
		/// <param name="token">The cancel token.</param>
		/// <returns>API key.</returns>
		Task<string> RetrieveKey(CancellationToken token = default);
	}
}
