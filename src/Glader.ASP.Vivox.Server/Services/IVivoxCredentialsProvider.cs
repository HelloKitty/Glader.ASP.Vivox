using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.ASP.Vivox
{
	public interface IVivoxCredentialsProvider
	{
		string VivoxIssuer { get; }

		string VivoxDomain { get; }
	}
}
