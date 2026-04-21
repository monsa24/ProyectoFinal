using System.Data.Entity;
using TiendaOnline.Models;

namespace TiendaOnline.App_Start
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            IdentitySeeder.Seed(context);

            base.Seed(context);
        }
    }
}