using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaIllico.Api.Domains.Authentications;
using PizzaIllico.Api.Domains.Authentications.Actions;
using PizzaIllico.Api.Domains.Authentications.Actions.Credentials;
using PizzaIllico.Dtos.Authentications;
using PizzaIllico.Dtos.Authentications.Credentials;
using Storm.Api.Controllers;
using Storm.Api.Dtos;
using Storm.Api.Swaggers.Attributes;

namespace PizzaIllico.Api.Controllers
{
	public class AuthenticationController : BaseController
	{
		public AuthenticationController(IServiceProvider services) : base(services)
		{
		}

		[HttpPost]
		[Route(Urls.REFRESH_TOKEN)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response<LoginResponse>), HttpStatusCode.OK)]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
		{
			return await Action<RefreshTokenCommand, RefreshTokenCommandParameter, LoginResponse>(new RefreshTokenCommandParameter
			{
				RefreshToken = request.RefreshToken,
				ClientId = request.ClientId,
				ClientSecret = request.ClientSecret
			});
		}

		[HttpPost]
		[Route(Urls.LOGIN_WITH_CREDENTIALS)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response<LoginResponse>), HttpStatusCode.OK)]
		public async Task<IActionResult> LoginWithCredentials([FromBody] LoginWithCredentialsRequest request)
		{
			return await Action<LoginWithCredentialsCommand, LoginWithCredentialsCommandParameter, LoginResponse>(new LoginWithCredentialsCommandParameter
			{
				Login = request.Login,
				Password = request.Password,
				ClientId = request.ClientId,
				ClientSecret = request.ClientSecret
			});
		}

		[HttpPatch]
		[Route(Urls.SET_PASSWORD)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response), HttpStatusCode.OK)]
		public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
		{
			return await Action<SetPasswordCommand, SetPasswordCommandParameter>(new SetPasswordCommandParameter
			{
				Data = request
			});
		}
	}
}