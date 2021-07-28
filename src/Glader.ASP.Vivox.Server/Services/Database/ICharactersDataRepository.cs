using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// Contract for data repository that provides information related to characters and sessions.
	/// </summary>
	public interface ICharactersDataRepository
	{
		/// <summary>
		/// Retrieves the current character associated with the <see cref="accountId"/>.
		/// </summary>
		/// <param name="accountId">The id of the account.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>The character id associated with the account.</returns>
		Task<int> RetrieveCharacterIdByAccountIdAsync(int accountId, CancellationToken token = default);

		/// <summary>
		/// Indicates if the provided <see cref="accountId"/> has an active character session.
		/// </summary>
		/// <param name="accountId">The id of the account.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>True if the account has a session.</returns>
		Task<bool> AccountHasActiveSessionAsync(int accountId, CancellationToken token = default);

		/// <summary>
		/// Retrieves the character's session info from the <see cref="characterId"/>.
		/// </summary>
		/// <param name="characterId">The character's id.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>The session information required.</returns>
		Task<ICharacterSessionInfo> RetrieveSessionInfoAsync(int characterId, CancellationToken token = default);
	}
}
