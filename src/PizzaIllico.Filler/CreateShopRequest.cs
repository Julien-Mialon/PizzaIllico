using System.Collections.Generic;
using Newtonsoft.Json;

namespace PizzaIllico.Filler
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