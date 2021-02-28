using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Accounts.Converters;
using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Api.Domains.Accounts.Services;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Authentications.Services;
using PizzaIllico.Api.Domains.Bases.Actions;
using PizzaIllico.Dtos.Accounts;
using Storm.Api.Core.Exceptions;
using Storm.Api.Core.Validations;

namespace PizzaIllico.Api.Domains.Accounts.Actions
{
	public class SetUserProfileCommandParameter
	{
		public SetUserProfileRequest Data { get; set; }
	}

	public class SetUserProfileCommand : AuthenticatedAction<SetUserProfileCommandParameter, UserProfileResponse>
	{
		public SetUserProfileCommand(IServiceProvider services) : base(services)
		{
		}

		protected override bool ValidateParameter(SetUserProfileCommandParameter parameter)
		{
			return base.ValidateParameter(parameter) &&
				parameter.Data.IsNotNull() &&
				parameter.Data.Email.IsNotNullOrEmpty();
		}

		protected override void PrepareParameter(SetUserProfileCommandParameter parameter)
		{
			base.PrepareParameter(parameter);

			parameter.Data.Email = parameter.Data.Email.ToLowerInvariant().Trim();
		}

		protected override async Task<UserProfileResponse> Action(SetUserProfileCommandParameter parameter)
		{
			IUserService userService = Services.GetService<IUserService>();
			User existingAccount = await userService.GetUser(parameter.Data.Email);

			if (existingAccount is {} && existingAccount.Id != Account.Id)
			{
				throw new DomainException(Errors.EMAIL_ALREADY_EXISTS, "This email is already used by another account");
			}

			bool requireLoginUpdate = Account.Email != parameter.Data.Email;
			Account.Email = parameter.Data.Email;
			Account.FirstName = parameter.Data.FirstName;
			Account.LastName = parameter.Data.LastName;
			Account.PhoneNumber = parameter.Data.PhoneNumber;
			
			await userService.UpdateUser(Account);

			if (requireLoginUpdate)
			{
				ICredentialAuthenticationService authenticationService = Services.GetService<ICredentialAuthenticationService>();
				LoginPasswordUserAuthentication credentials = await authenticationService.GetAuthenticationForUser(Account);
				if (credentials is {})
				{
					credentials.Login = Account.Email;
					await authenticationService.UpdateAuthentication(credentials);
				}
			}

			return Account.ToDtos();
		}
	}
}