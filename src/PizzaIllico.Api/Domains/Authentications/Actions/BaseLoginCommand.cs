using System;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Authentications.Converters;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using PizzaIllico.Dtos.Authentications;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Extensions;
using Storm.Api.Core.Validations;

namespace PizzaIllico.Api.Domains.Authentications.Actions
{
	public interface ILoginCommandParameter
	{
		string ClientId { get; }
		string ClientSecret { get; }
	}
	
	public abstract class BaseLoginCommand<TParameter> : BaseAction<TParameter, LoginResponse>
		where TParameter : ILoginCommandParameter
	{
		protected BaseLoginCommand(IServiceProvider services) : base(services)
		{
		}

		protected override bool ValidateParameter(TParameter parameter)
		{
			return base.ValidateParameter(parameter) && 
				parameter.ClientId.IsNotNullOrEmpty() && 
				parameter.ClientSecret.IsNotNullOrEmpty();
		}

		protected override async Task<LoginResponse> Action(TParameter parameter)
		{
			var authenticationService = Resolve<IAuthenticationService>();

			ApiClient apiClient = await authenticationService.FindClient(parameter.ClientId, parameter.ClientSecret);
			apiClient.UnauthorizedIfNull();

			IAuthentication authentication = await LoadAuthentication(parameter);
			authentication.UnauthorizedIfNull();

			AuthenticationToken token = await authenticationService.CreateToken(apiClient, authentication);

			return token.ToDtos();
		}

		protected abstract Task<IAuthentication> LoadAuthentication(TParameter parameter);
	}
}