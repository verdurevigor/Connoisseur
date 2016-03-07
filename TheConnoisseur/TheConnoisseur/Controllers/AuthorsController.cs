using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;
using Microsoft.AspNet.Identity;

namespace TheConnoisseur.Controllers
{
    public class AuthorsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // Author profile page
        // GET: Authors
        public ActionResult Index()
        {
            // Pass currently logged in user to the view
            var author = db.Users.Find(User.Identity.GetUserId());
            return View(author);
        }

        // TODO: use this method when a friend's profile link is clicked.
        // GET: Authors/FriendProfile/1
        public ActionResult FriendProfile(string friendID)
        {
            var friend = (from a in db.Users
                            where a.Id == friendID
                            select a).FirstOrDefault();
            // Public profile
            if (friend.PrivacyType == 1)
            {
                return View("Index", friend);
            }
            
            // For non-public profiles, ensure friendship has positive relation
            var you = db.Users.Find(User.Identity.GetUserId());

            var relationship = (from f in db.Friendships
                            where f.AuthorID1 == you.Id
                            && f.AuthorID2 == friend.Id
                            select f.Relation).FirstOrDefault();

            if (relationship == true)
            {
                return View("Index", friend);
            }
            // Private profile and not friends
            return View("PrivateProfile");
        }

        [ChildActionOnly]
        public ActionResult FriendsList()
        {
            // TODO: take 5 friends at most
            var friends = (from a in db.Users
                           join f in db.Friendships on a.Id equals f.AuthorID1
                           where f.Relation == true
                           select a).ToList();

            return PartialView(friends);
        }

        [ChildActionOnly]
        public ActionResult BeerList(string authorId)
        {
            // Grab three most recent beer journals with authorId
            var beers = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == authorId).OrderBy(b => b.Journal.Date).Take(3);

            return PartialView(beers);
        }

        [ChildActionOnly]
        public ActionResult CoffeeList(string authorId)
        {
            // Grab three most recent coffee journals with authorId
            var coffees = db.Coffees.Include("Journal").Where(b => b.Journal.Author.Id == authorId).OrderBy(b => b.Journal.Date).Take(3);

            return PartialView(coffees);
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
