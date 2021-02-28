using PizzaIllico.Api.Domains.Pizzas.Models;
using PizzaIllico.Dtos.Pizzas;

namespace PizzaIllico.Api.Domains.Pizzas.Converters
{
    public static class DtosConverters
    {
        public static ShopItem ToDtos(this Shop shop)
        {
            return new ShopItem
            {
                Id = shop.Id,
                Address = shop.Address,
                Latitude = shop.Latitude,
                Longitude = shop.Longitude,
                Name = shop.Name,
                MinutesPerKilometer = shop.MinutesPerKilometer
            };
        }

        public static PizzaItem ToDtos(this Pizza pizza)
        {
            return new PizzaItem
            {
                Id = pizza.Id,
                Description = pizza.Description,
                Name = pizza.Name,
                Price = pizza.Price,
                OutOfStock = pizza.OutOfStock,
            };
        }

        public static OrderItem ToDtos(this Order order, Shop shop)
        {
            return new OrderItem
            {
                Amount = order.Amount,
                Date = order.EntityCreatedDate,
                Shop = shop.ToDtos()
            };
        }
    }
}