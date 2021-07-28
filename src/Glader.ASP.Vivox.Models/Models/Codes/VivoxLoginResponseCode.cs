using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// Enumeration of all responses for a vivox login attempt.
	/// </summary>
	public enum VivoxLoginResponseCode
	{
		/// <summary>
		/// Indicates the request was successful.
		/// </summary>
		Success = GladerEssentialsModelConstants.RESPONSE_CODE_SUCCESS_VALUE,

		GeneralServerError = 2,

		/// <summary>
		/// Indicates that no session character session is actually active for the account.
		/// </summary>
		NoActiveCharacterSession = 3,

		/// <summary>
		/// This can happen if they attempt to join channels they don't belong in.
		/// </summary>
		ChannelUnavailable = 4,
	}
}