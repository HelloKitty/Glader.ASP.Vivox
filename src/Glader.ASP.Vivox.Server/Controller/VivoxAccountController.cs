﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Glader.ASP.Vivox;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GladMMO
{
	[Route("api/[controller]")]
	public class VivoxAccountController : AuthorizationReadyController
	{
		public VivoxAccountController(IClaimsPrincipalReader claimsReader, ILogger<AuthorizationReadyController> logger) 
			: base(claimsReader, logger)
		{

		}

		[AuthorizeJwt]
		[NoResponseCache]
		[HttpPost("Login")]
		public async Task<ResponseModel<string, VivoxLoginResponseCode>> LoginVivox([FromServices] ITrinityCharactersRepository characterRepository,
			[FromServices] IFactoryCreatable<VivoxTokenClaims, VivoxTokenClaimsCreationContext> claimsFactory,
			[FromServices] IVivoxTokenSignService signService)
		{
			int accountId = this.ClaimsReader.GetAccountId<int>(User);

			//If the user doesn't actually have a claimed session in the game
			//then we shouldn't log them into Vivox.
			if (!await characterRepository.AccountHasActiveSessionAsync(accountId))
				return Failure<string, VivoxLoginResponseCode>(VivoxLoginResponseCode.NoActiveCharacterSession);

			int characterId = await RetrieveSessionCharacterIdAsync(characterRepository, accountId);

			VivoxTokenClaims claims = claimsFactory.Create(new VivoxTokenClaimsCreationContext(characterId, VivoxAction.Login));

			//We don't send it back in a JSON form even though it's technically a JSON object
			//because the client just needs it as a raw string anyway to put through the Vivox client API.
			return Success<string, VivoxLoginResponseCode>(signService.CreateSignature(claims));
		}

		private static async Task<int> RetrieveSessionCharacterIdAsync(ITrinityCharactersRepository characterRepository, int accountId)
		{
			//TODO: Technically a race condition here.
			return await characterRepository.RetrieveCharacterIdByAccountIdAsync(accountId);
		}
	}
}