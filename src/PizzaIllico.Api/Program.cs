using Storm.Api.Launchers;

namespace PizzaIllico.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DefaultLauncher<Startup>.RunWebHost(args);
        }
    }
}