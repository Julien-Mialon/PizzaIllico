using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Api.Domains.Authentications.Models;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Extensions;
using Storm.Api.Core.Services;
using Storm.Api.Extensions;

namespace PizzaIllico.Api.Domains.Authentications.Services
{
	public class ActionTokenAuthenticator : BaseService, IActionAuthenticator<User, object>
	{
		public ActionTokenAuthenticator(IServiceProvider services) : base(services)
		{
		}
		
		public async Task<(bool authenticated, User account)> Authenticate(object _)
		{
			IHttpContextAccessor context = Services.GetService<IHttpContextAccessor>();

			if (!context.TryGetHeaderOrQueryParameter("Authorization", "Authorization", out string authorizationValue))
			{
				return (false, null);
			}

			if (authorizationValue.IsNullOrEmpty() || !authorizationValue.StartsWith("Bearer "))
			{
				return (false, null);
			}

			string accessToken = authorizationValue.Substring("Bearer ".Length);
			IAuthenticationService authenticationService = Services.GetService<IAuthenticationService>();

			(AuthenticationToken token, User user) = await authenticationService.GetTokenByAccess(accessToken);
			if (token is null || user is null)
			{
				return (false, null);
			}

			if (DateTime.UtcNow > token.ExpirationDate)
			{
				return (false, null);
			}
			
			return (true, user);
		}
	}
}