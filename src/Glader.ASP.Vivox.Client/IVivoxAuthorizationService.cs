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
		[Post("/api/VivoxAccount/Login")]
		Task<ResponseModel<string, VivoxLoginResponseCode>> LoginAsync(CancellationToken token = default);

		[RequiresAuthentication]
		[Post("/api/VivoxChannel/proximity/join")]
		Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinProximityChatAsync(CancellationToken token = default);

		[RequiresAuthentication]
		[Post("/api/VivoxChannel/guild/join")]
		Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinGuildChatAsync(CancellationToken token = default);
	}
}