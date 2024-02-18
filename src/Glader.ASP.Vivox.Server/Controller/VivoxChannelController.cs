using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Glader.ASP.Vivox;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Glader.ASP.Vivox
{
	[Route("api/[controller]")]
	public class VivoxChannelController : AuthorizationReadyController
	{
		public VivoxChannelController(IClaimsPrincipalReader claimsReader, ILogger<AuthorizationReadyController> logger) 
			: base(claimsReader, logger)
		{

		}

		[AuthorizeJwt]
		[NoResponseCache]
		[HttpPost("proximity/join")]
		public async Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinProximityChatAsync(
			[FromServices] ICharactersDataRepository characterRepository,
			[FromServices] IVivoxClaimsTokenFactory claimsFactory,
			[FromServices] IVivoxTokenSignService signService, CancellationToken token = default)
		{
			int accountId = this.ClaimsReader.GetAccountId<int>(User);

			//If the user doesn't actually have a claimed session in the game
			//then we shouldn't log them into Vivox.
			if (!await characterRepository.AccountHasActiveSessionAsync(accountId, token))
				return Failure<VivoxChannelJoinResponse, VivoxLoginResponseCode>(VivoxLoginResponseCode.NoActiveCharacterSession);

			int characterId = await RetrieveSessionCharacterIdAsync(characterRepository, accountId);

			ICharacterSessionInfo session = await characterRepository.RetrieveSessionInfoAsync(characterId, token);

			//Players in the same zone will all join the same proximity channel such as Prox-1.
			//They can use this for proximity text and voice chat.
			//TODO: Support cases where a character is put into a different map than the DB says it should be in
			//TODO: Support instances, right now all instances share the same channel.
			//TODO: Use a factory for channel name generation maybe?
			VivoxTokenClaims claims = claimsFactory.Create(new VivoxTokenClaimsCreationContext(characterId, VivoxAction.JoinChannel, new VivoxChannelData(true, new MapChannelNameBuilder(true, session.MapId).ToString())));

			//We don't send it back in a JSON form even though it's technically a JSON object
			//because the client just needs it as a raw string anyway to put through the Vivox client API.
			return Success<VivoxChannelJoinResponse, VivoxLoginResponseCode>(new VivoxChannelJoinResponse(await signService.CreateSignatureAsync(claims, token), claims.DestinationSIPURI));
		}

		/// <summary>
		/// Call this before calling <see cref="JoinProximityChatAsync"/> to check if you're able to join the proximity voice chat channel yet.
		/// </summary>
		/// <param name="mapId">The map id to check joining availability.</param>
		/// <param name="token">Cancel token.</param>
		/// <returns>True if the provided <see cref="mapId"/> is available to join via <see cref="JoinProximityChatAsync"/>.</returns>
		[AuthorizeJwt]
		[NoResponseCache]
		[HttpGet("proximity/{mapId}/available")]
		public async Task<bool> CanJoinProximityChatAsync([FromRoute(Name = "mapId")] int mapId,
			[FromServices] ICharactersDataRepository characterRepository,
			CancellationToken token = default)
		{
			int accountId = this.ClaimsReader.GetAccountId<int>(User);

			//If the user doesn't actually have a claimed session in the game
			//then they cannot log into the channel
			if (!await characterRepository.AccountHasActiveSessionAsync(accountId, token))
				return false;

			try
			{
				int characterId = await RetrieveSessionCharacterIdAsync(characterRepository, accountId);
				ICharacterSessionInfo session = await characterRepository.RetrieveSessionInfoAsync(characterId, token);

				// If they're in the map that they are checking that they want to join
				// then we should say they can, the client doesn't know the instance id so don't bother checking it.
				return session.MapId == mapId;
			}
			catch (Exception e)
			{
				if (Logger.IsEnabled(LogLevel.Error))
					Logger.LogError($"Failed to check if Account: {accountId} was allowed to join Proximity Chat Channel MapId: {mapId}. Reason: {e}");

				return false;
			}
		}

		private static async Task<int> RetrieveSessionCharacterIdAsync(ICharactersDataRepository characterRepository, int accountId)
		{
			//TODO: Technically a race condition here.
			return await characterRepository.RetrieveCharacterIdByAccountIdAsync(accountId);
		}
	}
}