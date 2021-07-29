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
			return services.AddTransient<ICharactersDataRepository, TDataRepositoryType>();
		}
	}
}
