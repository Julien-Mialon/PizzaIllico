using System.Collections.Generic;
using Storm.SqlMigrations;

namespace PizzaIllico.SqlMigrations
{
    public class PizzaMigrations : BaseMigrationModule
    {
        public override List<IMigration> Operations { get; } = new List<IMigration>
        {
            new Migration01()
        };

        public PizzaMigrations() : base(nameof(PizzaMigrations))
        {
        }
    }
}