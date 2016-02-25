using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;
using TheConnoisseur.ViewModels;

namespace TheConnoisseur.Controllers
{
    // This annotation ensures that anyone can access the AuthController
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AuthController()
            : this(Startup.UserManagerFactory.Invoke())
        {
        }

        public AuthController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        // GET: /Auth/LogIn
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        // POST: /Auth/LogIn
        [HttpPost]
        public ActionResult LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Attempt to find the user.
            var user = userManager.Find(model.Email, model.Password);
            
            if (user != null)
            {
                // If user exists, create a claims identity for the user that can be passed
                // to AuthenticationManager. This will include any custom claims that are stored.
                // Sign in the user using the cookie authentication middleware.
                SignIn(user);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new AppUser
            {
                UserName = model.Email,
                State = model.State,
                City = model.City
            };

            var result = userManager.Create(user, model.Password);

            if (result.Succeeded)
            {
                // If user exists, create a claims identity for the user that can be passed
                // to AuthenticationManager. This will include any custom claims that are stored.
                // Sign in the user using the cookie authentication middleware.
                SignIn(user);
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private void SignIn(AppUser user)
        {
            var identity = userManager.CreateIdentity(
            user, DefaultAuthenticationTypes.ApplicationCookie);
            
            GetAuthenticationManager().SignIn(identity);
         }

        private string GetRedirectUrl(string returnUrl)
        {
            // Ensure that the return URL is not blank and also that is is a URL from this website
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        // TODO: if references remain at zero after full Identity implementation remove this method
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}