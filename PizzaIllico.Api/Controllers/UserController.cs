using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaIllico.Api.Domains.Accounts;
using PizzaIllico.Api.Domains.Accounts.Actions;
using PizzaIllico.Dtos.Accounts;
using PizzaIllico.Dtos.Authentications;
using Storm.Api.Controllers;
using Storm.Api.Dtos;
using Storm.Api.Swaggers.Attributes;

namespace PizzaIllico.Api.Controllers
{
	public class UserController : BaseController
	{
		public UserController(IServiceProvider services) : base(services)
		{
		}

		[HttpGet]
		[Route(Urls.USER_PROFILE)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response<UserProfileResponse>), HttpStatusCode.OK)]
		public async Task<IActionResult> UserProfile()
		{
			return await Action<UserProfileQuery, UserProfileQueryParameter, UserProfileResponse>(new UserProfileQueryParameter());
		}

		[HttpPatch]
		[Route(Urls.SET_USER_PROFILE)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response<UserProfileResponse>), HttpStatusCode.OK)]
		public async Task<IActionResult> SetUserProfile([FromBody] SetUserProfileRequest request)
		{
			return await Action<SetUserProfileCommand, SetUserProfileCommandParameter, UserProfileResponse>(new SetUserProfileCommandParameter
			{
				Data = request
			});
		}
		
		[HttpPost]
		[Route(Urls.CREATE_USER)]
		[Category(Urls.CATEGORY)]
		[Response(typeof(Response<LoginResponse>), HttpStatusCode.OK)]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
		{
			return await Action<CreateUserCommand, CreateUserCommandParameter, LoginResponse>(new CreateUserCommandParameter
			{
				Data = request
			});
		}
	}
}