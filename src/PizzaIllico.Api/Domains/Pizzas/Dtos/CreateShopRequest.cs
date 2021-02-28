using System.Collections.Generic;
using Newtonsoft.Json;
using PizzaIllico.Dtos.Pizzas;

namespace PizzaIllico.Api.Domains.Pizzas.Dtos
{
    public class CreateShopRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("shop")]
        public ShopItem Shop { get; set; }
        
        [JsonProperty("pizzas")]
        public List<CreatePizzaRequest> Pizzas { get; set; }
    }
}