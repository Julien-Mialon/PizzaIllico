using Storm.Api.Core.Models;

namespace PizzaIllico.Api.Domains.Pizzas.Models
{
    public class Shop : BaseEntityWithAutoIncrement
    {
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        public double MinutesPerKilometer { get; set; }
    }
}