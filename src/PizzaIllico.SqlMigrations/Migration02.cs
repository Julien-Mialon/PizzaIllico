using System;
using System.Data;
using System.Threading.Tasks;
using PizzaIllico.Api.Domains.Accounts.Models;
using PizzaIllico.Api.Domains.Authentications.Models;
using PizzaIllico.Api.Domains.Pizzas.Models;
using ServiceStack.OrmLite;
using Storm.SqlMigrations;

namespace PizzaIllico.SqlMigrations
{
    public class Migration02 : BaseMigration
    {
        public Migration02() : base(2)
        {
        }

        public override async Task Apply(IDbConnection db)
        {
            await db.InsertAsync(new ApiClient
            {
                ClientId = "MOBILE",
                ClientSecret = "UNIV",
                CollationId = Guid.NewGuid(),
            });
        }
    }
}