using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Accounts.Services;

namespace PizzaIllico.Api.Domains.Accounts
{
	public static class AccountBootstrapper
	{
		public static IServiceCollection UseAccountModule(this IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			
			return services;
		}
	}
}