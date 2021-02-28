using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PizzaIllico.Dtos.Pizzas
{
    public class OrderItem
    {
        [JsonProperty("shop")]
        public ShopItem Shop { get; set; }
        
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}