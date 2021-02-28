using System;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using Storm.Api.Core.Validations;

namespace PizzaIllico.Api.Domains.Authentications.Actions.Credentials
{
	public class LoginWithCredentialsCommandParameter : ILoginCommandParameter
	{
		public string Login { get; set; }
		
		public string Password { get; set; }
		
		public string ClientId { get; set; }
		
		public string ClientSecret { get; set; }
	}
	
	public class LoginWithCredentialsCommand : BaseLoginCommand<LoginWithCredentialsCommandParameter>
	{
		public LoginWithCredentialsCommand(IServiceProvider services) : base(services)
		{
		}

		protected override bool ValidateParameter(LoginWithCredentialsCommandParameter parameter)
		{
			return base.ValidateParameter(parameter) && 
				parameter.Login.IsNotNullOrEmpty() && 
				parameter.Password.IsNotNullOrEmpty();
		}

		protected override async Task<IAuthentication> LoadAuthentication(LoginWithCredentialsCommandParameter parameter)
		{
			var authenticationService = Resolve<ICredentialAuthenticationService>();
			return await authenticationService.LoginWithCredentials(parameter.Login, parameter.Password);
		}
	}
}