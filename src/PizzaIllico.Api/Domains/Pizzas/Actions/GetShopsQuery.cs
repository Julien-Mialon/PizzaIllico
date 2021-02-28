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
    public class GetShopsQueryParameter
    {
        
    }
    
    public class GetShopsQuery : BaseAction<GetShopsQueryParameter, List<ShopItem>>
    {
        public GetShopsQuery(IServiceProvider services) : base(services)
        {
        }

        protected override async Task<List<ShopItem>> Action(GetShopsQueryParameter parameter)
        {
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            List<Shop> shops= await connection.From<Shop>()
                .NotDeleted()
                .AsSelectAsync(connection);

            return shops.ConvertAll(x => x.ToDtos());
        }
    }
}