using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaIllico.Api.Domains.Pizzas;
using PizzaIllico.Api.Domains.Pizzas.Actions;
using PizzaIllico.Api.Domains.Pizzas.Dtos;
using PizzaIllico.Dtos.Pizzas;
using Storm.Api.Controllers;
using Storm.Api.Dtos;
using Storm.Api.Swaggers.Attributes;

namespace PizzaIllico.Api.Controllers
{
    public class PizzaController : BaseController
    {
        public PizzaController(IServiceProvider services) : base(services)
        {
        }

        [HttpGet]
        [Route(Urls.LIST_SHOPS)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(Response<List<ShopItem>>), HttpStatusCode.OK)]
        public async Task<IActionResult> ListShops()
        {
            return await Action<GetShopsQuery, GetShopsQueryParameter, List<ShopItem>>(new GetShopsQueryParameter());
        }

        [HttpGet]
        [Route(Urls.LIST_PIZZA)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(Response<List<PizzaItem>>), HttpStatusCode.OK)]
        public async Task<IActionResult> ListPizzas([FromRoute] long shopId)
        {
            return await Action<GetPizzasQuery, GetPizzasQueryParameter, List<PizzaItem>>(new GetPizzasQueryParameter
            {
                ShopId = shopId
            });
        }

        [HttpGet]
        [Route(Urls.LIST_ORDERS)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(Response<List<OrderItem>>), HttpStatusCode.OK)]
        public async Task<IActionResult> ListOrders()
        {
            return await Action<GetOrdersQuery, GetOrdersQueryParameter, List<OrderItem>>(new GetOrdersQueryParameter());
        }

        [HttpPost]
        [Route(Urls.DO_ORDER)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(Response<OrderItem>), HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromRoute] long shopId, [FromBody] CreateOrderRequest request)
        {
            return await Action<CreateOrderCommand, CreateOrderCommandParameter, OrderItem>(new CreateOrderCommandParameter
            {
                ShopId = shopId,
                PizzaIds = request?.PizzaIds
            });
        }

        [HttpGet]
        [Route(Urls.GET_IMAGE)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(byte[]), HttpStatusCode.OK)]
        public async Task<IActionResult> GetPizzaImage([FromRoute] long shopId, [FromRoute] long pizzaId)
        {
            return await FileAction<GetPizzaImageQuery, GetPizzaImageQueryParameter>(new GetPizzaImageQueryParameter
            {
                ShopId = shopId,
                PizzaId = pizzaId,
            });
        }

        /// <summary>
        /// Do not use this route from app !
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(Urls.CREATE_SHOP)]
        [Category(Urls.CATEGORY)]
        [Response(typeof(Response<ShopItem>), HttpStatusCode.OK)]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request)
        {
            return await Action<CreateShopCommand, CreateShopCommandParameter, ShopItem>(new CreateShopCommandParameter
            {
                Data = request
            });
        }
    }
}