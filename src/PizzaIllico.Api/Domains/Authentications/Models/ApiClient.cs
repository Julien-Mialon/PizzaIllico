using ServiceStack.DataAnnotations;
using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Authentications.Models
{
	[Alias("ApiClients")]
	public class ApiClient : BaseEntityWithAutoIncrement
	{
		[Index]
		[StringLength(64)]
		public string ClientId { get; set; }
		
		[StringLength(64)]
		public string ClientSecret { get; set; }
	}
}