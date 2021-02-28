using System.Data;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Pizzas.Models;
using ServiceStack.OrmLite;
using Storm.SqlMigrations;

namespace PizzaIllico.SqlMigrations
{
    public class Migration01 : BaseMigration
    {
        public Migration01() : base(1)
        {
        }

        public override Task Apply(IDbConnection db)
        {
            db.CreateTable<User>();
            db.CreateTable<ApiClient>();
            db.CreateTable<LoginPasswordUserAuthentication>();
            db.CreateTable<AuthenticationToken>();
            db.CreateTable<Shop>();
            db.CreateTable<Pizza>();
            db.CreateTable<Order>();

            return Task.CompletedTask;
        }
    }
}