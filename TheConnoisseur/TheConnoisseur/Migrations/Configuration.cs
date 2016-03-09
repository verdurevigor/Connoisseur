namespace TheConnoisseur.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using TheConnoisseur.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TheConnoisseur.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppDbContext context)
        {
            // Turn on debugging (be sure to add a breakpoint somewhere in this Seed method)
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();


            // UserManager is used to add and modify Identities
            var userManager = new UserManager<Author>(
                new UserStore<Author>(
                    new AppDbContext()));

            /* The Members and Role have been successfully added. Currently the Authors being used in the Journal entries are being pulled from the database explicitly below the commented section.
            // Create Authors and add them to database
            Author admin = new Author()
            {
                City = "The World",
                State = "UN",
                DateCreated = new DateTime(2000, 1, 1, 0, 0, 0),
                Email = "admin@email.com",
                FavItem = "Blackest coffees.",
                FirstName = "Admin",
                LastName = "Admin",
                PrivacyType = 2,
                Tagline = "I know what you drank last summer.",
                UserName = "Admin",
                AvatarPath = "~/Content/Image/newavatar.png"
            };

            Author a1 = new Author()
            {
                City = "Eugene",
                State = "OR",
                DateCreated = new DateTime(2012, 2, 8, 0, 0, 0),
                Email = "brody@email.com",
                FavItem = "Sumatran Coffee",
                FirstName = "Brody",
                LastName = "Case",
                PrivacyType = 1,
                Tagline = "Coffee coffee - buzz buzz Buzz!",
                UserName = "Brodster",
                AvatarPath = "~/Content/Image/newavatar.png"
            };

            Author a2 = new Author()
            {
                City = "Portland",
                State = "OR",
                DateCreated = new DateTime(2014, 3, 17, 0, 0, 0),
                Email = "will@email.com",
                FavItem = "The beers",
                FirstName = "Will",
                LastName = "Dewald",
                PrivacyType = 3,
                Tagline = "Beer + cats are a fun time",
                UserName = "PhilosophicalDrinker",
                AvatarPath = "~/Content/Image/newavatar.png"
            };

            Author a3 = new Author()
            {
                City = "Eugene",
                State = "OR",
                DateCreated = new DateTime(2014, 7, 1, 0, 0, 0),
                Email = "kaja@email.com",
                FavItem = "N/A",
                FirstName = "Kaja",
                LastName = "Blackwell",
                PrivacyType = 1,
                Tagline = "Summertime is only a pint away.",
                UserName = "Flowerchild",
                AvatarPath = "~/Content/Image/newavatar.png"
            };

            
            // Add users with userManager
            userManager.Create(admin, "password");
            userManager.Create(a1, "password");
            userManager.Create(a2, "password");
            userManager.Create(a3, "password");

            // Get user Ids explicitly to add role to the user
            string aid = (from u in db.Users
                          where u.UserName == admin.UserName
                          select u.Id).FirstOrDefault();

            // Generate Role
            db.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            db.SaveChanges();
            // Assign Roles to Authors
            userManager.AddToRole(aid, "Admin");
            context.SaveChanges();                  // TODO: Investigate why context.SaveChanges() is used when db.SaveChanges() and userManager are used here.
            */

            // This section of Author querying is only for use if the Users have been created but the Journal/Friendship has not.
            var a1 = (from a in context.Users where a.UserName == "Brodster" select a).FirstOrDefault();
            var a2 = (from a in context.Users where a.UserName == "PhilosophicalDrinker" select a).FirstOrDefault();
            var a3 = (from a in context.Users where a.UserName == "Flowerchild" select a).FirstOrDefault();

            /* Completed
            // Only Perform this code block once!
            // Generate friend relationships and one blocked relationship
            Friendship f1a = new Friendship() { AuthorID1 = a1.Id, AuthorID2 = a2.Id, Relation = true };
            Friendship f1b = new Friendship() { AuthorID1 = a2.Id, AuthorID2 = a1.Id, Relation = true };
            Friendship f2a = new Friendship() { AuthorID1 = a1.Id, AuthorID2 = a3.Id, Relation = true };
            Friendship f2b = new Friendship() { AuthorID1 = a3.Id, AuthorID2 = a1.Id, Relation = true };
            Friendship f3 = new Friendship() { AuthorID1 = a3.Id, AuthorID2 = a2.Id, Relation = false };
            context.Friendships.Add(f1a);
            context.Friendships.Add(f1b);
            context.Friendships.Add(f2a);
            context.Friendships.Add(f2b);
            context.Friendships.Add(f3);
            SaveChanges(context);
            */


            // Generate Journals (with Author), then Subtype (coffee, beer) and set the Journal into the Subtype before saving.
            Journal j1 = new Journal()
            {
                Author = a1,
                Date = new DateTime(2013, 2, 9, 17, 12, 0),
                Description = "Delicious single-hop beer! Citrus notes are present and it leaves a slightly astringent aftertaste. Perfect!",
                ImagePath = "~/Content/Images/beerglass.png",
                JType = 2,
                Location = "Beer Stein",
                Maker = "Hopworks Urban Brewing",
                PrivacyType = 1,
                Rating = 5,
                Title = "IPX - Simcoe Hop"
            };

            Beer b1 = new Beer()
            {
                Abv = 6.9m,
                Ibu = 112,
                Journal = j1
            };

            Journal j2 = new Journal()
            {
                Author = a1,
                Date = new DateTime(2013, 2, 15, 12, 20, 0),
                Description = "Great dark beer that isn't too heavy, but still delivers",
                ImagePath = "~/Content/Images/beerglass.png",
                JType = 2,
                Location = "Home",
                Maker = "Deschuttes",
                PrivacyType = 1,
                Rating = 4,
                Title = "Black Butte Porter"
            };

            Beer b2 = new Beer()
            {
                Abv = 5.9m,
                Ibu = 40,
                Journal = j2
            };

            Journal j3 = new Journal()
            {
                Author = a1,
                Date = new DateTime(2013, 2, 16, 7, 12, 0),
                Description = "Too much of a bright, citrus tone; maybe it would be good made as cold brew.",
                ImagePath = "~/Content/Images/hotcup.png",
                JType = 1,
                Location = "Home",
                Maker = "Wandering Goat",
                PrivacyType = 1,
                Rating = 3,
                Title = "Yirgacheffe"
            };

            Coffee c3 = new Coffee()
            {
                FoodPairing = "Brownies",
                Journal = j3,
                Origin = "Ethiopia",
                RoastType = "Not as dark as I'd prefer."
            };

            Journal j4 = new Journal()
            {
                Author = a3,
                Date = new DateTime(2013, 2, 20, 7, 12, 0),
                Description = "Yum! I love the mild bitterness and the cocoa undertones",
                ImagePath = "~/Content/Images/hotcup.png",
                JType = 1,
                Location = "Sixteen Tons",
                Maker = "Wandering Goat",
                PrivacyType = 1,
                Rating = 3,
                Title = "Hair of the Goat"
            };

            Coffee c4 = new Coffee()
            {
                Journal = j4,
                Origin = "Latin American and Indonesia",
                RoastType = "Medium"
            };

            Journal j5 = new Journal()
            {
                Author = a2,
                Date = new DateTime(2015, 10, 11, 17, 19, 0),
                Description = "Fresh harvested spruce tips add a refined Northwest flavor to the refreshingly crisp IPA.",
                ImagePath = "~/Content/Images/beerglass.png",
                JType = 2,
                Location = "Home",
                Maker = "My Dad!",
                PrivacyType = 1,
                Rating = 3,
                Title = "Spruce Tips IPA Batch No. 2"
            };

            Beer b5 = new Beer()
            {
                Abv = 7.2m,
                Ibu = 97,
                Journal = j5
            };

            // Add the object to database
            context.Journals.AddOrUpdate(j => j.Description, j1, j2, j3, j4, j5);
            context.Beers.AddOrUpdate(b => b.Ibu, b1, b2, b5);
            context.Coffees.AddOrUpdate(c => c.Origin, c3, c4);

            /* Only do this part once: Completed
            // Add definition terms for privacy types
            Privacy p1 = new Privacy() { PrivacyID = 1, Name = "Public" };
            Privacy p2 = new Privacy() { PrivacyID = 2, Name = "Private" };
            Privacy p3 = new Privacy() { PrivacyID = 3, Name = "Friends Only" };
            context.Privacies.AddOrUpdate(p => p.Name, p1, p2, p3);
             * */
            SaveChanges(context);
        }

        // This custome method displays detailed error output to the console.
        private static void SaveChanges(AppDbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
        }
    }
}
