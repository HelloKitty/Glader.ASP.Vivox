using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.ASP.Vivox
{
	public sealed class StaticVivoxCredentialsProvider : IVivoxCredentialsProvider
	{
		public string VivoxIssuer { get; }

		public string VivoxDomain { get; }

		public StaticVivoxCredentialsProvider(string vivoxIssuer, string vivoxDomain)
		{
			if (string.IsNullOrWhiteSpace(vivoxIssuer)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(vivoxIssuer));
			if (string.IsNullOrWhiteSpace(vivoxDomain)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(vivoxDomain));

			VivoxIssuer = vivoxIssuer;
			VivoxDomain = vivoxDomain;
		}
	}
}
