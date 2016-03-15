using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheConnoisseur.Models;
using TheConnoisseur.Migrations;

namespace TheConnoisseur.Models
{
    public class AppDbContext : IdentityDbContext<Author>
    {
        public AppDbContext() : base("DefaultConnection")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Journal> Journals { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Beer> Beers { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Coffee> Coffees { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Privacy> Privacies { get; set; }

        public System.Data.Entity.DbSet<TheConnoisseur.Models.Friendship> Friendships { get; set; }
        public System.Data.Entity.DbSet<TheConnoisseur.Models.FriendshipPending> FriendshipsPending { get; set; }
    }
}