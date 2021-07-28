using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Glader.ASP.Vivox;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GladMMO
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
		public async Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinZoneProximityChat(
			[FromServices] ICharactersDataRepository characterRepository,
			[FromServices] IFactoryCreatable<VivoxTokenClaims, VivoxTokenClaimsCreationContext> claimsFactory,
			[FromServices] IVivoxTokenSignService signService)
		{
			int accountId = this.ClaimsReader.GetAccountId<int>(User);

			//If the user doesn't actually have a claimed session in the game
			//then we shouldn't log them into Vivox.
			if (!await characterRepository.AccountHasActiveSessionAsync(accountId))
				return Failure<VivoxChannelJoinResponse, VivoxLoginResponseCode>(VivoxLoginResponseCode.NoActiveCharacterSession);

			int characterId = await RetrieveSessionCharacterIdAsync(characterRepository, accountId);

			ICharacterSessionInfo session = await characterRepository.RetrieveSessionInfoAsync(characterId);

			//Players in the same zone will all join the same proximity channel such as Prox-1.
			//They can use this for proximity text and voice chat.
			//TODO: Support cases where a character is put into a different map than the DB says it should be in
			//TODO: Support instances, right now all instances share the same channel.
			//TODO: Use a factory for channel name generation maybe?
			VivoxTokenClaims claims = claimsFactory.Create(new VivoxTokenClaimsCreationContext(characterId, VivoxAction.JoinChannel, new VivoxChannelData(true, $"Prox-{session.MapId}")));

			//We don't send it back in a JSON form even though it's technically a JSON object
			//because the client just needs it as a raw string anyway to put through the Vivox client API.
			return Success<VivoxChannelJoinResponse, VivoxLoginResponseCode>(new VivoxChannelJoinResponse(signService.CreateSignature(claims), claims.DestinationSIPURI));
		}

		/*[AuthorizeJwt]
		[NoResponseCache]
		[HttpPost("guild/join")]
		public async Task<ResponseModel<VivoxChannelJoinResponse, VivoxLoginResponseCode>> JoinGuildChat(
			[FromServices] ITrinityCharactersRepository characterSessionRepository,
			[FromServices] IFactoryCreatable<VivoxTokenClaims, VivoxTokenClaimsCreationContext> claimsFactory,
			[FromServices] ITrinityGuildMemberRepository guildMembershipRepository,
			[FromServices] IVivoxTokenSignService signService)
		{
			int accountId = this.ClaimsReader.GetAccountId<int>(User);

			//If the user doesn't actually have a claimed session in the game
			//then we shouldn't log them into Vivox.
			if(!await characterSessionRepository.AccountHasActiveSessionAsync(accountId))
				return BuildFailedResponseModel(VivoxLoginResponseCode.NoActiveCharacterSession);

			int characterId = await RetrieveSessionCharacterIdAsync(characterSessionRepository, accountId);

			if(!await guildMembershipRepository.ContainsAsync((uint) characterId))
				return BuildFailedResponseModel(VivoxLoginResponseCode.ChannelUnavailable);

			int guildId = (int) (await guildMembershipRepository.RetrieveAsync((uint) characterId)).Guildid;

			//TODO: Use a factory for channel name generation maybe?
			VivoxTokenClaims claims = claimsFactory.Create(new VivoxTokenClaimsCreationContext(characterId, VivoxAction.JoinChannel, new VivoxChannelData(false, $"Guild-{guildId}")));

			//We don't send it back in a JSON form even though it's technically a JSON object
			//because the client just needs it as a raw string anyway to put through the Vivox client API.
			return BuildSuccessfulResponseModel(new VivoxChannelJoinResponse(signService.CreateSignature(claims), claims.DestinationSIPURI));
		}*/

		private static async Task<int> RetrieveSessionCharacterIdAsync(ICharactersDataRepository characterRepository, int accountId)
		{
			//TODO: Technically a race condition here.
			return await characterRepository.RetrieveCharacterIdByAccountIdAsync(accountId);
		}
	}
}