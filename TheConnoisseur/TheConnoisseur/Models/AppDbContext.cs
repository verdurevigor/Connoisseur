using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class AppDbContext : IdentityDbContext<Author>
    {
        public AppDbContext()
            : base("TheConnoisseurContext")
        {
        }
        /* Include custom models in the database. Because Author inherits from Identity user it does not need to be 
           included here because it is the generic type of the IdentityContext class which is inherited by the AppDbContext 
           as coded in the class header */
        public System.Data.Entity.DbSet<TheConnoisseur.Models.Journal> Journals { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Beer> Beers { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Coffee> Coffees { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Privacy> Privacies { get; set; }
        public System.Data.Entity.DbSet<TheConnoisseur.Models.Journal> Friendships { get; set; }
    }
}