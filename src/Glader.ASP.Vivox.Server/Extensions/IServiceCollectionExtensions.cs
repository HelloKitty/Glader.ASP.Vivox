using System;
using System.Collections.Generic;
using System.Text;
using Glader.ASP.Vivox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Glader.ASP.Vivox
{
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		/// Registers a <see cref="ICharactersDataRepository"/>
		/// in the provided <see cref="services"/>.
		/// </summary>
		/// <param name="services">Service container.</param>
		/// <returns></returns>
		public static IServiceCollection RegisterVivoxCharacterDataRepository<TDataRepositoryType>(this IServiceCollection services) 
			where TDataRepositoryType : class, ICharactersDataRepository
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			return services.AddTransient<ICharactersDataRepository, TDataRepositoryType>();
		}

		/// <summary>
		/// Registers a <see cref="IVivoxTokenSignService"/> and <see cref="IVivoxAPIKeyRepository"/>
		/// in the provided <see cref="services"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="instance">Optional instance to use instead of using DI to create it.</param>
		/// <returns></returns>
		public static IServiceCollection RegisterVivoxTokenServices<TVivoxSignServiceType, TVivoxAPIKeyRepositoryType, TVivoxClaimsTokenFactoryType, TVivoxCredentialsProviderType>(this IServiceCollection services,
			TVivoxCredentialsProviderType instance = null)
			where TVivoxSignServiceType : class, IVivoxTokenSignService
			where TVivoxAPIKeyRepositoryType : class, IVivoxAPIKeyRepository
			where TVivoxClaimsTokenFactoryType : class, IVivoxClaimsTokenFactory
			where TVivoxCredentialsProviderType : class, IVivoxCredentialsProvider
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services = services.AddTransient<IVivoxTokenSignService, TVivoxSignServiceType>()
				.AddTransient<IVivoxAPIKeyRepository, TVivoxAPIKeyRepositoryType>()
				.AddTransient<IVivoxClaimsTokenFactory, TVivoxClaimsTokenFactoryType>();

			if (instance == default)
				services = services.AddTransient<IVivoxCredentialsProvider, TVivoxCredentialsProviderType>();
			else
				services = services.AddSingleton<IVivoxCredentialsProvider>(instance);

			return services;
		}

		/// <summary>
		/// Registers a <see cref="IVivoxTokenSignService"/> and <see cref="IVivoxAPIKeyRepository"/>
		/// in the provided <see cref="services"/>.
		/// Using <see cref="DefaultLocalVivoxTokenSigningService"/> for <see cref="IVivoxTokenSignService"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="instance">Optional instance to use instead of using DI to create it.</param>
		/// <returns></returns>
		public static IServiceCollection RegisterVivoxTokenServices<TVivoxAPIKeyRepositoryType, TVivoxClaimsTokenFactoryType, TVivoxCredentialsProviderType>(this IServiceCollection services,
			TVivoxCredentialsProviderType instance = null)
			where TVivoxAPIKeyRepositoryType : class, IVivoxAPIKeyRepository
			where TVivoxClaimsTokenFactoryType : class, IVivoxClaimsTokenFactory
			where TVivoxCredentialsProviderType : class, IVivoxCredentialsProvider
		{
			return RegisterVivoxTokenServices<DefaultLocalVivoxTokenSigningService, TVivoxAPIKeyRepositoryType, TVivoxClaimsTokenFactoryType, TVivoxCredentialsProviderType>(services, instance);
		}
	}
}
