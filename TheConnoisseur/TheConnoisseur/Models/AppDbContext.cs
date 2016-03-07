using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class AppDbContext : IdentityDbContext<Author>
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Journal> Journals { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Beer> Beers { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Coffee> Coffees { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Privacy> Privacies { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Friendship> Friendships { get; set; }
    }
}