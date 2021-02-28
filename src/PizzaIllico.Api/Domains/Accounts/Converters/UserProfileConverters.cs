using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Dtos.Accounts;

namespace PizzaIllico.Api.Domains.Accounts.Converters
{
	public static class UserProfileConverters
	{
		public static UserProfileResponse ToDtos(this User user)
		{
			return new UserProfileResponse
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber
			};
		}
	}
}