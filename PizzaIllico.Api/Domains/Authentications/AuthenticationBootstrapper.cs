using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using Storm.Api.Core.CQRS;

namespace PizzaIllico.Api.Domains.Authentications
{
	public static class AuthenticationBootstrapper
	{
		public static IServiceCollection UseAuthenticationModule(this IServiceCollection services)
		{
			services.AddScoped<IAuthenticationService, AuthenticationService>()
				.AddScoped<ICredentialAuthenticationService, CredentialAuthenticationService>()
				.AddScoped<IActionAuthenticator<User, object>, ActionTokenAuthenticator>();
			
			return services;
		}
	}
}