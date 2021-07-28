using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Glader.ASP.Vivox
{
	[JsonObject]
	public record VivoxChannelJoinResponse(string AuthToken, string ChannelUri);
}