using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TiendaOnline;
using TiendaOnline.App_Start;
using TiendaOnline.Models;

namespace TiendaOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new DbInitializer());

            using (var db = new ApplicationDbContext())
            {
                db.Database.Initialize(false);
            }
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            // Aquí podrías registrar el error en un log o base de datos
            // Por simplicidad, redirigimos a una página de error genérica
            Response.Clear();
            Server.ClearError();
            Response.Redirect("~/Error");
        }
    }
}