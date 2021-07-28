using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.ASP.Vivox
{
	/// <summary>
	/// Data provider for the character's session information.
	/// </summary>
	public interface ICharacterSessionInfo
	{
		/// <summary>
		/// The id of the map the character is within.
		/// </summary>
		int MapId { get; set; }

		/// <summary>
		/// The optional instance id of the map the character is within.
		/// </summary>
		int InstanceId { get; set; }
	}
}
