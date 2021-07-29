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
		/// <returns></returns>
		public static IServiceCollection RegisterVivoxTokenServices<TVivoxSignServiceType, TVivoxAPIKeyRepositoryType>(this IServiceCollection services)
			where TVivoxSignServiceType : class, IVivoxTokenSignService
			where TVivoxAPIKeyRepositoryType : class, IVivoxAPIKeyRepository
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			return services.AddTransient<IVivoxTokenSignService, TVivoxSignServiceType>()
				.AddTransient<IVivoxAPIKeyRepository, TVivoxAPIKeyRepositoryType>();
		}

		/// <summary>
		/// Registers a <see cref="IVivoxTokenSignService"/> and <see cref="IVivoxAPIKeyRepository"/>
		/// in the provided <see cref="services"/>.
		/// Using <see cref="DefaultLocalVivoxTokenSigningService"/> for <see cref="IVivoxTokenSignService"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterVivoxTokenServices<TVivoxAPIKeyRepositoryType>(this IServiceCollection services)
			where TVivoxAPIKeyRepositoryType : class, IVivoxAPIKeyRepository
		{
			if(services == null) throw new ArgumentNullException(nameof(services));
			return services.AddTransient<IVivoxTokenSignService, DefaultLocalVivoxTokenSigningService>()
				.AddTransient<IVivoxAPIKeyRepository, TVivoxAPIKeyRepositoryType>();
		}
	}
}
