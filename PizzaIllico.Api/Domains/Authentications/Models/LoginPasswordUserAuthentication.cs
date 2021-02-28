using PizzaIllico.Api.Domains.Accounts.Models;
using ServiceStack.DataAnnotations;
using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Authentications.Models
{
	[Alias("LoginPasswordUserAuthentications")]
	public class LoginPasswordUserAuthentication : BaseEntityWithAutoIncrement, IAuthentication
	{
		[References(typeof(User))]
		public long UserId { get; set; }
		
		[Index]
		[StringLength(256)]
		public string Login { get; set; }
		
		public string Password { get; set; }
	}
}