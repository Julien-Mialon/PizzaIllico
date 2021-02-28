using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PizzaIllico.Api.Domains.Pizzas.Models;
using ServiceStack.OrmLite;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Databases;
using Storm.Api.Core.Domains.Results;
using Storm.Api.Core.Extensions;

namespace PizzaIllico.Api.Domains.Pizzas.Actions
{
    public class GetPizzaImageQueryParameter
    {
        public long ShopId { get; set; }
        
        public long PizzaId { get; set; }
    }
    
    public class GetPizzaImageQuery : BaseAction<GetPizzaImageQueryParameter, FileResult>
    {
        public GetPizzaImageQuery(IServiceProvider services) : base(services)
        {
        }

        protected override async Task<FileResult> Action(GetPizzaImageQueryParameter parameter)
        {
            IDbConnection connection = await Services.GetService<IDatabaseService>().Connection;

            Pizza pizza = await connection.From<Pizza>()
                .Where(x => x.ShopId == parameter.ShopId && x.Id == parameter.PizzaId)
                .NotDeleted()
                .AsSingleAsync(connection);

            pizza.NotFoundIfNull();
            
            return FileResult.Create(pizza.Image, FileContentType.CONTENT_TYPE_JPG, "image.jpg");
        }
    }
}