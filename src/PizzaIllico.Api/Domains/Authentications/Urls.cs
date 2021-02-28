namespace PizzaIllico.Api.Domains.Authentications
{
	public static class Urls
	{
		private const string ROOT = "api/v1/authentication";
		public const string CATEGORY = "Authentication";

		public const string REFRESH_TOKEN = ROOT + "/refresh";

		public const string LOGIN_WITH_CREDENTIALS = ROOT + "/credentials";
		public const string SET_PASSWORD = ROOT + "/credentials/set";
	}
}