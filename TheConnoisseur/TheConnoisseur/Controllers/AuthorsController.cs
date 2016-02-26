using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Controllers
{
    // This private controller inherits from AppController to gain access to
    // the current user.
    public class AuthorsController : Controller
    {
        private AppDbContext db = new AppDbContext();   // TODO: this might need to be changed back to TheConnoisseurContext

        // GET: Authors
        public ActionResult Index()
        {
            // Retrieve author information from the currently signed in user.
            Author author = (from a in db.Users         // TODO: check that this retrieves an author appropriately from the db. If not meddle with the DbContext above.
                        where a.Id == User.Identity.GetUserId()
                        select a).FirstOrDefault();
            return View(author);
        }

        // TODO: use this method when a friend's profile link is clicked.
        // GET: Authors/FriendProfile/1
        public ActionResult FriendProfile(int friendID)
        {
            Author author = (from u in db.Users
                             join f in db.Friendships on u.Id equals f.AuthorId2
                             where f. == friendID
                             select a).FirstOrDefault();
            // TODO: ensure that author found has "friend" relation with current identity
            return View("Index", author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
