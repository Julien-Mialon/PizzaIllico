using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Bases.Actions;
using PizzaIllico.Api.Domains.Pizzas.Converters;
using PizzaIllico.Api.Domains.Pizzas.Models;
using PizzaIllico.Dtos.Pizzas;
using ServiceStack.OrmLite;
using Storm.Api.Core.Databases;
using Storm.Api.Core.Exceptions;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Pizzas.Actions
{
    public class CreateOrderCommandParameter
    {
        public long ShopId { get; set; }
        public List<long> PizzaIds { get; set; }
    }
    
    public class CreateOrderCommand : AuthenticatedAction<CreateOrderCommandParameter, OrderItem>
    {
        public CreateOrderCommand(IServiceProvider services) : base(services)
        {
        }

        protected override bool ValidateParameter(CreateOrderCommandParameter parameter)
        {
            return base.ValidateParameter(parameter) &&
                   parameter.PizzaIds?.Count > 0;
        }

        protected override async Task<OrderItem> Action(CreateOrderCommandParameter parameter)
        {
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            Shop shop = await connection.From<Shop>().NotDeleted().Where(x => x.Id == parameter.ShopId).AsSingleAsync(connection);
            shop.NotFoundIfNull();

            Dictionary<long, Pizza> pizzas = (await connection.From<Pizza>()
                    .Where(x => x.ShopId == shop.Id)
                    .AsSelectAsync(connection))
                .ToDictionary(x => x.Id);

            double amount = 0;
            
            foreach (long id in parameter.PizzaIds)
            {
                if (pizzas.TryGetValue(id, out Pizza pizza))
                {
                    if (pizza.OutOfStock)
                    {
                        throw new DomainException(Errors.PIZZA_OUT_OF_STOCK, $"Pizza with id {id} is out of stock and therefore can not be ordered");
                    }
                    
                    amount += pizza.Price;
                }
                else
                {
                    throw new DomainException(Errors.PIZZA_NOT_EXISTS, $"Pizza with id {id} does not exists in shop");
                }
            }
            
            Order order = new Order
            {
                CollationId = Guid.NewGuid(),
                ShopId = shop.Id,
                UserId = Account.Id,
                Amount = amount
            };

            await connection.InsertAsync(order);

            return order.ToDtos(shop);
        }
    }
}