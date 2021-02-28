using Newtonsoft.Json;

namespace PizzaIllico.Api.Domains.Pizzas.Dtos
{
    public class CreatePizzaRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }
        
        [JsonProperty("out_of_stock")]
        public bool OutOfStock { get; set; }
        
        [JsonProperty("image")]
        public byte[] Image { get; set; }
    }
}