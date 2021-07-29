using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Glader.Essentials;
using GladMMO;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Glader.ASP.Vivox
{
	public static class IMvcBuilderExtensions
	{
		/// <summary>
		/// Registers the <see cref="VivoxAccountController"/> with the MVC
		/// controllers. See controller documentation for what it does and how it works.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMvcBuilder RegisterVivoxControllers(this IMvcBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			return builder.RegisterController<VivoxAccountController>()
				.RegisterController<VivoxChannelController>();
		}
	}
}
