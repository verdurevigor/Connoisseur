using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TheConnoisseur.Migrations;
using TheConnoisseur.Models;

namespace TheConnoisseur
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // No longer used; Migration is implemented.
            // When application is started, each time recreate the database. It is populated with mock data in the ConnoisseurDbInitializer class
            //Database.SetInitializer(new TheConnoisseurDbInitializer());
            
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        
        }
    }
}
