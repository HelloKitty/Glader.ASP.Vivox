using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glader.ASP.Vivox;
using Glader.Essentials;
using Refit;

namespace Glader.ASP.Vivox
{
	//TODO: Automate user-agent SDK version headers
	[Headers("User-Agent: Glader")]
	public interface IVivoxAuthorizationService
	{
		[RequiresAuthentication]
		[Post("/api/VivoxAccount/login")]
		Task<ResponseModel<string, VivoxLoginResponseCode>> LoginAsync(CancellationToken token = default);

		[RequiresAuthentication]
		[Post("/api/VivoxChannel/proximity/join")]
		Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinProximityChatAsync(CancellationToken token = default);

		/// <summary>
		/// Call this before calling <see cref="JoinProximityChatAsync"/> to check if you're able to join the proximity voice chat channel yet.
		/// </summary>
		/// <param name="mapId">The map id to check joining availability.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>True if the provided <see cref="mapId"/> is available to join via <see cref="JoinProximityChatAsync"/>.</returns>
		[RequiresAuthentication]
		[Get("/api/VivoxChannel/proximity/{mapId}/available")]
		Task<bool> CanJoinProximityChatAsync([AliasAs("mapId")] int mapId, CancellationToken token = default);
	}
}