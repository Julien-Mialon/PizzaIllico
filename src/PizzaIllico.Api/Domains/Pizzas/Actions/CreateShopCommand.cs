using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Pizzas.Converters;
using PizzaIllico.Api.Domains.Pizzas.Dtos;
using PizzaIllico.Api.Domains.Pizzas.Models;
using PizzaIllico.Dtos.Pizzas;
using ServiceStack.OrmLite;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Databases;
using Storm.Api.Core.Exceptions;

namespace PizzaIllico.Api.Domains.Pizzas.Actions
{
    public class CreateShopCommandParameter
    {
        public CreateShopRequest Data { get; set; }
    }
    
    public class CreateShopCommand : BaseAction<CreateShopCommandParameter, ShopItem>
    {
        public CreateShopCommand(IServiceProvider services) : base(services)
        {
        }

        protected override async Task<ShopItem> Action(CreateShopCommandParameter parameter)
        {
            if (parameter.Data.Token != "p7Ds2dGps2UDudkY")
            {
                throw new DomainHttpCodeException(HttpStatusCode.NotFound, "route does not exists");
            }
            
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            Shop shop = new Shop
            {
                Address = parameter.Data.Shop.Address,
                Name = parameter.Data.Shop.Name,
                Latitude = parameter.Data.Shop.Latitude,
                Longitude = parameter.Data.Shop.Longitude,
                MinutesPerKilometer = parameter.Data.Shop.MinutesPerKilometer,
                CollationId = Guid.NewGuid()
            };

            shop.Id = await connection.InsertAsync(shop, selectIdentity: true);

            List<Pizza> toCreate = parameter.Data.Pizzas.ConvertAll(x => new Pizza
            {
                ShopId = shop.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Image = x.Image,
                OutOfStock = x.OutOfStock,
                CollationId = Guid.NewGuid(),
            });
            
            await connection.InsertAllAsync(toCreate);

            return shop.ToDtos();
        }
    }
}