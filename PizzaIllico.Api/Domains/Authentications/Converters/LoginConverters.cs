using System;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Dtos.Authentications;

namespace PizzaIllico.Api.Domains.Authentications.Converters
{
	public static class LoginConverters
	{
		public static LoginResponse ToDtos(this AuthenticationToken authenticationToken)
		{
			return new LoginResponse
			{
				AccessToken = authenticationToken.AccessToken,
				RefreshToken = authenticationToken.RefreshToken,
				ExpiresIn = (int)authenticationToken.ExpirationDate.Subtract(DateTime.UtcNow).TotalSeconds,
				TokenType = authenticationToken.TokenType
			};
		}
	}
}