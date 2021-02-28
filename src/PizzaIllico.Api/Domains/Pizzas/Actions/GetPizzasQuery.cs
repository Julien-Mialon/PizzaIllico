using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Pizzas.Converters;
using PizzaIllico.Api.Domains.Pizzas.Models;
using PizzaIllico.Dtos.Pizzas;
using ServiceStack.OrmLite;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Databases;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Pizzas.Actions
{
    public class GetPizzasQueryParameter
    {
        public long ShopId { get; set; }
    }
    
    public class GetPizzasQuery : BaseAction<GetPizzasQueryParameter, List<PizzaItem>>
    {
        public GetPizzasQuery(IServiceProvider services) : base(services)
        {
        }

        protected override async Task<List<PizzaItem>> Action(GetPizzasQueryParameter parameter)
        {
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            Shop shop = await connection.From<Shop>()
                .NotDeleted()
                .Where(x => x.Id == parameter.ShopId)
                .AsSingleAsync(connection);
            shop.NotFoundIfNull();
            
            var pizzas = await connection.From<Pizza>()
                .NotDeleted()
                .Where(x => x.ShopId == parameter.ShopId)
                .AsSelectAsync(connection);

            return pizzas.ConvertAll(x => x.ToDtos());
        }
    }
}