using System;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Authentications.Converters;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using PizzaIllico.Dtos.Authentications;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Authentications.Actions
{
	public class RefreshTokenCommandParameter
	{
		public string RefreshToken { get; set; }
		
		public string ClientId { get; set; }
		
		public string ClientSecret { get; set; }
	}
	
	public class RefreshTokenCommand : BaseAction<RefreshTokenCommandParameter, LoginResponse>
	{
		public RefreshTokenCommand(IServiceProvider services) : base(services)
		{
		}

		protected override async Task<LoginResponse> Action(RefreshTokenCommandParameter parameter)
		{
			IAuthenticationService authenticationService = Resolve<IAuthenticationService>();
			ApiClient client = await authenticationService.FindClient(parameter.ClientId, parameter.ClientSecret);
			client.UnauthorizedIfNull();

			AuthenticationToken token = await authenticationService.GetTokenByRefresh(parameter.RefreshToken, client);
			token.UnauthorizedIfNull();

			AuthenticationToken newToken = await authenticationService.RefreshToken(token);

			return newToken.ToDtos();
		}
	}
}