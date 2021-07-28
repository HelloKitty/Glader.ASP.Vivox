using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.ASP.Vivox
{
	public interface ITrinityCharactersRepository
	{
		Task<int> RetrieveCharacterIdByAccountIdAsync(int accountId, CancellationToken token = default);

		Task<bool> AccountHasActiveSessionAsync(int accountId, CancellationToken token = default);

		Task<ICharacterSessionInfo> RetrieveSessionInfoAsync(int characterId, CancellationToken token = default);
	}
}
