using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// The response model for successfully joining a Vivox channel.
	/// </summary>
	[JsonObject]
	public record VivoxChannelJoinResponse(string AuthToken, string ChannelUri);
}