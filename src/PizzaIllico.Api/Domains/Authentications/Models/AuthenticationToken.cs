using System;
using PizzaIllico.Api.Domains.Accounts.Models;
using ServiceStack.DataAnnotations;
using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Authentications.Models
{
	[Alias("AuthenticationTokens")]
	public class AuthenticationToken : BaseEntityWithAutoIncrement
	{
		[References(typeof(User))]
		public long UserId { get; set; }
		
		[References(typeof(ApiClient))]
		public long ApiClientId { get; set; }
		
		[Index]
		[StringLength(64)]
		public string AccessToken { get; set; }
		
		[Index]
		[StringLength(64)]
		public string RefreshToken { get; set; }
		
		public string TokenType { get; set; }
		
		public DateTime ExpirationDate { get; set; }
	}
}