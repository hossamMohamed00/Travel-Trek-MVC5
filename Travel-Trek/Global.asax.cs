using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Travel_Trek.Db_Context;
using Travel_Trek.Models;

namespace Travel_Trek
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using (ApplicationDbContext Db = new ApplicationDbContext())
            {
                //* Check if there is no roles
                if (!Db.UserRoles.Any())
                {
                    //* Create roles
                    var admin = new UserRole();
                    var agency = new UserRole();
                    var traveler = new UserRole();

                    admin.Name = "Admin";
                    agency.Name = "Agency";
                    traveler.Name = "Travller";

                    Db.UserRoles.Add(admin);
                    Db.UserRoles.Add(agency);
                    Db.UserRoles.Add(traveler);

                    Db.SaveChanges();
                }

            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
