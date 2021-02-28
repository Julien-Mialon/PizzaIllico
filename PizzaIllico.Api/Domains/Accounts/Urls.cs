namespace PizzaIllico.Api.Domains.Accounts
{
	public static class Urls
	{
		private const string ROOT = "api/v1/accounts";
		public const string CATEGORY = "Account";

		public const string USER_PROFILE = ROOT + "/me";
		public const string CREATE_USER = ROOT + "/register";
		public const string SET_USER_PROFILE = ROOT + "/me";
	}
}