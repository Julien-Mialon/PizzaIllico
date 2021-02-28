namespace PizzaIllico.Api.Domains.Pizzas
{
    public static class Urls
    {
        private const string ROOT = "api/v1";
        public const string CATEGORY = "Shop";

        public const string LIST_SHOPS = ROOT + "/shops";
        public const string CREATE_SHOP = ROOT + "/shops";
        public const string LIST_PIZZA = ROOT + "/shops/{shopId}/pizzas";
        public const string GET_IMAGE = ROOT + "/shops/{shopId}/pizzas/{pizzaId}/image";
        public const string DO_ORDER = ROOT + "/shops/{shopId}";
        public const string LIST_ORDERS = ROOT + "/orders";
    }
}