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

		/// <summary>
		/// The id of the map instance.
		/// </summary>
		public int InstanceId { get; }

		public MapChannelNameBuilder(bool isProximity, int mapId, int instanceId)
		{
			// Map zero is considered valid (example, EK)
			if (mapId < 0) throw new ArgumentOutOfRangeException(nameof(mapId));

			// InstanceId will be 0 potentiall for continents or other non-instanced maps
			if (instanceId < 0) throw new ArgumentOutOfRangeException(nameof(instanceId));

			IsProximity = isProximity;
			MapId = mapId;
			InstanceId = instanceId;
		}

		/// <summary>
		/// Provides the channel name.
		/// </summary>
		/// <returns>Channel name.</returns>
		public override string ToString()
		{
			if (IsProximity)
				return $"Prox-{MapId}-{InstanceId}";

			throw new NotImplementedException($"TODO: Determine how non-proximity mapid-based channels should be named");
		}
	}
}
