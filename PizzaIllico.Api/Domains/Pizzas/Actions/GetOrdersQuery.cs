using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Bases.Actions;
using PizzaIllico.Api.Domains.Pizzas.Converters;
using PizzaIllico.Api.Domains.Pizzas.Models;
using PizzaIllico.Dtos.Pizzas;
using ServiceStack.OrmLite;
using Storm.Api.Core.Databases;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Pizzas.Actions
{
    public class GetOrdersQueryParameter
    {
        
    }
    
    public class GetOrdersQuery : AuthenticatedAction<GetOrdersQueryParameter, List<OrderItem>>
    {
        public GetOrdersQuery(IServiceProvider services) : base(services)
        {
        }

        protected override async Task<List<OrderItem>> Action(GetOrdersQueryParameter parameter)
        {
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            List<(Order order, Shop shop)> orders = await connection.From<Order>()
                .LeftJoin<Order, Shop>((order, shop) => order.ShopId == shop.Id)
                .NotDeleted()
                .Where(x => x.UserId == Account.Id)
                .AsSelectMultiAsync(connection, connection.Mapper<Order, Shop, (Order order, Shop shop)>((order, shop) => (order, shop)));

            return orders.ConvertAll(x => x.order.ToDtos(x.shop));
        }
    }
}