using System;
using System.Collections.Generic;
using System.Text;

namespace Glader
{
	/// <summary>
	/// Used to create channel names for map-based channels.
	/// </summary>
	public sealed class MapChannelNameBuilder
	{
		/// <summary>
		/// Indicates if the channel is proximity based.
		/// </summary>
		public bool IsProximity { get; }

		/// <summary>
		/// Indicates the mapid.
		/// </summary>
		public int MapId { get; }

		public MapChannelNameBuilder(bool isProximity, int mapId)
		{
			// Map zero is considered valid (example, EK)
			if (mapId < 0) throw new ArgumentOutOfRangeException(nameof(mapId));

			IsProximity = isProximity;
			MapId = mapId;
		}

		public override string ToString()
		{
			if (IsProximity)
				return $"Prox-{MapId}";

			throw new NotImplementedException($"TODO: Determine how non-proximity mapid-based channels should be named");
		}
	}
}
