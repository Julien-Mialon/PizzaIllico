using System;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Accounts.Converters;
using PizzaIllico.Api.Domains.Bases.Actions;
using PizzaIllico.Dtos.Accounts;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Accounts.Actions
{
	public class UserProfileQueryParameter
	{
		
	}
	
	public class UserProfileQuery : AuthenticatedAction<UserProfileQueryParameter, UserProfileResponse>
	{
		public UserProfileQuery(IServiceProvider services) : base(services)
		{
		}

		protected override Task<UserProfileResponse> Action(UserProfileQueryParameter parameter)
		{
			return Account.ToDtos().AsTask();
		}
	}
}