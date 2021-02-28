using ServiceStack.DataAnnotations;
using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Pizzas.Models
{
    public class Pizza : BaseEntityWithAutoIncrement
    {
        [References(typeof(Shop))]
        public long ShopId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public double Price { get; set; }
        
        public byte[] Image { get; set; }
        
        public bool OutOfStock { get; set; }
    }
}