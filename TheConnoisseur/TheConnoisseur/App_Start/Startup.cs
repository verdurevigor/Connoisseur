using Antlr.Runtime.Misc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Security.Claims;
using TheConnoisseur.Models;

namespace TheConnoisseur
{
    public class Startup
    {
        public static Func<UserManager<Author>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Auth/login")
            });

            // configure the user manager
            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<Author>(
                    new UserStore<Author>(new AppDbContext()));
                // allow alphanumeric characters in username
                usermanager.UserValidator = new UserValidator<Author>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                // use custom claims provider
                usermanager.ClaimsIdentityFactory = new AuthorClaimsIdentityFactory();

                return usermanager;
            };
        }
    }
}