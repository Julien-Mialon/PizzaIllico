using PizzaIllico.Api.Domains.Accounts.Models;
using ServiceStack.DataAnnotations;
using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Pizzas.Models
{
    public class Order : BaseEntityWithAutoIncrement
    {
        [References(typeof(Shop))]
        public long ShopId { get; set; }
        
        [References(typeof(User))]
        public long UserId { get; set; }
        
        public double Amount { get; set; }
    }
}