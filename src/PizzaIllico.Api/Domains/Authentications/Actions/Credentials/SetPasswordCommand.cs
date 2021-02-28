using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Authentications.Extensions;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using PizzaIllico.Api.Domains.Bases.Actions;
using PizzaIllico.Dtos.Authentications.Credentials;
using Storm.Api.Core;
using Storm.Api.Core.Exceptions;
using Storm.Api.Core.Extensions;
using Storm.Api.Core.Validations;

namespace PizzaIllico.Api.Domains.Authentications.Actions.Credentials
{
	public class SetPasswordCommandParameter
	{
		public SetPasswordRequest Data { get; set; }
	}

	public class SetPasswordCommand : AuthenticatedAction<SetPasswordCommandParameter, Unit>
	{
		public SetPasswordCommand(IServiceProvider services) : base(services)
		{
		}

		protected override bool ValidateParameter(SetPasswordCommandParameter parameter)
		{
			return base.ValidateParameter(parameter) &&
			       parameter.Data.IsNotNull() &&
			       parameter.Data.NewPassword.IsNotNullOrEmpty();
		}

		protected override async Task<Unit> Action(SetPasswordCommandParameter parameter)
		{
			if (parameter.Data.NewPassword.NotStrongPassword())
			{
				throw new DomainException(Errors.WEAK_PASSWORD, "Password is too weak");
			}

			ICredentialAuthenticationService authenticationService = Services.GetService<ICredentialAuthenticationService>();
			LoginPasswordUserAuthentication credentials = await authenticationService.GetAuthenticationForUser(Account);

			if (credentials is null)
			{
				throw new DomainException(Errors.NO_CREDENTIALS, "No credentials for this user");
			}

			if (parameter.Data.OldPassword.AsSha256() != credentials.Password)
			{
				throw new DomainException(Errors.INVALID_PASSWORD, "Invalid old password");
			}

			await authenticationService.UpdatePassword(credentials, parameter.Data.NewPassword);
			return Unit.Default;
		}
	}
}