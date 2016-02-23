using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class TheConnoisseurDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<TheConnoisseurContext>
    {
        protected override void Seed(TheConnoisseurContext context)
        {
            Author a1 = new Author()
            {
                AuthorID = 1,
                AvatarPath = "",    // Empty for now
                City = "Eugene",
                DateCreated = new DateTime(2016, 1, 30, 12, 23, 45),
                Email = "brody@gmail.com",
                EmailConfirm = "brody@gmail.com",
                FavItem = "Heart of Darkness by the Wandering Goat",
                FirstName = "Brody",
                LastName = "Case",
                Password = "Test9!",
                PasswordConfirm = "Test9!",
                PrivacyType = 1,
                State = "OR",
                Tagline = "When it's bitter, it is best",
                Username = "BitterOne",
            };

            Author a2= new Author()
            {
                AuthorID = 2,
                AvatarPath = "",    // Empty for now
                City = "Portland",
                DateCreated = new DateTime(2016, 2, 3, 19, 12, 40),
                Email = "will@gmail.com",
                EmailConfirm = "will@gmail.com",
                FavItem = "Old Raspuin",
                FirstName = "Will",
                LastName = "Dewald",
                Password = "Test9!",
                PasswordConfirm = "Test9!",
                PrivacyType = 1,
                State = "OR",
                Tagline = "More rain, please.",
                Username = "SharingCat",
            };

            Journal j1 = new Journal()
            {
                AuthorID = 1,
                Date = new DateTime(2016, 2, 8, 16, 23, 4),
                Description = "With the earthy tones of a well roasted Sumatran coffee, the blackness of this cup was deep like my soul until together the taste and sensation brought my alive. For the light of heart, use less bean to water than normal.",
                ImagePath = "",
                JournalID = 1,
                JType = "co",
                Location = "At home",
                Maker = "Wander Goat",
                PrivacyType = 1,
                Rating = 4,
                Title = "Heart of Darkness",
            };

            Coffee c1 = new Coffee()
            {
                CoffeeID = 2,
                FoodPairing = "None",
                JournalID = 1,
                Origin = "Sumatra",
                RoastType = "Dark roast"
            };

            Privacy p1 = new Privacy() { PrivacyID = 1, Name = "public" };
            Privacy p2 = new Privacy() { PrivacyID = 2, Name = "private" };
            Privacy p3 = new Privacy() { PrivacyID = 3, Name = "friends only" };
            context.Privacies.Add(p1);
            context.Privacies.Add(p2);
            context.Privacies.Add(p3);

            context.Authors.Add(a1);
            context.Authors.Add(a2);

            context.Journals.Add(j1);
            context.Coffees.Add(c1);



            base.Seed(context);
        }
    }
}