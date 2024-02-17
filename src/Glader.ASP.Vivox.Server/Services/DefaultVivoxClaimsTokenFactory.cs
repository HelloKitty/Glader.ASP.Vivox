using System;
using System.Text;

namespace Glader.ASP.Vivox
{
	public sealed class DefaultVivoxClaimsTokenFactory : IVivoxClaimsTokenFactory
	{
		private IVivoxCredentialsProvider CredentialsProvider { get; }

		public DefaultVivoxClaimsTokenFactory(IVivoxCredentialsProvider credentialsProvider)
		{
			CredentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
		}

		public VivoxTokenClaims Create(VivoxTokenClaimsCreationContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			string actionType = ConvertActionType(context.Action);

			switch (context.Action)
			{
				case VivoxAction.Login:
					return new VivoxTokenClaims(CredentialsProvider.VivoxIssuer, ComputeExpiryTime(), actionType, 1, $"sip:.{CredentialsProvider.VivoxIssuer}.{context.CharacterId}.@{CredentialsProvider.VivoxDomain}", null, null);
				case VivoxAction.JoinChannel:
					return HandleChannelJoinTokenCreation(context, actionType);
				default:
					throw new NotImplementedException($"TODO: Implement token generation for VivoxAction: {context.Action}");
			}
			
		}

		private VivoxTokenClaims HandleChannelJoinTokenCreation(VivoxTokenClaimsCreationContext context, string actionType)
		{
			if (context.Channel == null)
				throw new ArgumentException($"Provided {nameof(VivoxTokenClaimsCreationContext)} for ChannelJoin lacks channel data.");

			//Properties for 3D audio
			//$"!p-{_audibleDistance}-{_conversationalDistance}-{_audioFadeIntensityByDistance.ToString("0.000", new System.Globalization.CultureInfo("en-US"))}-{(int) _audioFadeModel}";
			//Default values found in Vivox source
			/*_audibleDistance = 32;
			_conversationalDistance = 1;
			_audioFadeIntensityByDistance = 2.0f;
			_audioFadeModel = AudioFadeModel.InverseByDistance;*/
			string props = context.Channel.IsPositionalChannel ? $"!p-{96}-{4}-{1.0f.ToString("0.000", new System.Globalization.CultureInfo("en-US"))}-{(int)1}" : String.Empty;
			
			//From ChannelId in the Vivox API assembly: $"sip:confctl-{GetUriDesignator(_type)}-{_issuer}.{_name}{props}@{_domain}"
			string channelURI = $"sip:confctl-{(context.Channel.IsPositionalChannel ? "d" : "g")}-{CredentialsProvider.VivoxIssuer}.{context.Channel.ChannelName}{props}@{CredentialsProvider.VivoxDomain}";
			return new VivoxTokenClaims(CredentialsProvider.VivoxIssuer, ComputeExpiryTime(), actionType, 1, $"sip:.{CredentialsProvider.VivoxIssuer}.{context.CharacterId}.@{CredentialsProvider.VivoxDomain}", channelURI, null);
		}

		//We don't currently use this. It was a good idea but may not be supported as usernames for vivox.
		private unsafe string ComputeCharacterString(int contextCharacterId)
		{
			/*ObjectGuid<>
			ObjectGuid playerGuid = new ObjectGuidBuilder()
				.WithType(EntityTypeId.TYPEID_PLAYER)
				.WithId(contextCharacterId)
				.Build();*/

			//Access raw memory of the guid.
			ulong playerRawGuid = (ulong) contextCharacterId;
			byte* rawValue = (byte*) &playerRawGuid;

			//The idea here is we use the player's 64bit guid value directly
			//as the string character value. That way it can be moved to and from
			//the player guid efficiently.
			return Encoding.ASCII.GetString(rawValue, sizeof(ulong));
		}

		private static int ComputeExpiryTime()
		{
			//90 seconds is the example time found here: https://docs.vivox.com/v5/general/unity/5_1_0/Default.htm#AccessTokenDeveloperGuide/GeneratingTokensOnClientUnity.htm%3FTocPath%3DUnity%7CAccess%2520Token%2520Developer%2520Guide%7C_____6
			//This is basicallt from Vivox GetLoginToken. It's what they do with the provided TimeSpan.
			return (int) DateTimeOffset.UtcNow.AddSeconds(90).ToUnixTimeSeconds();
		}

		private static string ConvertActionType(VivoxAction action)
		{
			switch (action)
			{
				case VivoxAction.Login:
					return "login";
				case VivoxAction.JoinChannel:
					return "join";
				default:
					throw new NotImplementedException($"TODO: Implement string generation for VivoxAction: {action}");
			}
		}
	}
}