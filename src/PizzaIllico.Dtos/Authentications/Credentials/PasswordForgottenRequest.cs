using Newtonsoft.Json;

namespace PizzaIllico.Dtos.Authentications.Credentials
{
	public class PasswordForgottenRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
	}
}