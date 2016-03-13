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

        public ActionResult PrivateProfile()
        {
            return View("PrivateProfile");
        }

        // Author profile page
        // GET: Authors
        public ActionResult Index()
        {
            // Pass currently logged in user to the view
            var author = db.Users.Find(User.Identity.GetUserId());
            return View(author);
        }

        // GET: Authors/FriendProfile/1
        public ActionResult FriendProfile(string friendID)
        {
            var friend = (from a in db.Users
                            where a.Id == friendID
                            select a).FirstOrDefault();
            // User arrived with invalid parameter
            if (friend == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Public profile
            if (friend.PrivacyType == 1)
            {
                return View(friend);
            }
            // For non-public profiles, ensure friendship has positive relation
            if (CheckFriendship(friend))
            {
                return View(friend);
            }
            // Not friends
            return View("NotFriendsYet", friend);
        }

        [ChildActionOnly]
        public ActionResult FriendsList(string authorId)
        {
            // TODO: Ensure Take(5) functions properly
            var friends = (from a in db.Users
                           join f in db.Friendships on a.Id equals f.AuthorID1
                           where f.AuthorID2 == authorId && f.Relation == true 
                           select a).Take(5).ToList();
            
            return PartialView(friends);
        }

        // These List controller methods do not need to validate friendship because they are only invokable as a Child Action from
        // the FriendProfile view - which would not load in the first place if the profile is "friends only" and requester is not a friend
        [ChildActionOnly]
        public ActionResult BeersList(string authorId)
        {
            // Grab three most recent beer journals with authorId
            var beers = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == authorId).OrderBy(b => b.Journal.Date).Take(3).ToList();
            
            // Shorten the description for brief viewing
            // TODO: Check the display length of 200 characters and possibly increase it.
            foreach (Beer b in beers)
            {
                if (b.Journal.Description.Length > 115)
                    b.Journal.Description = b.Journal.Description.Substring(0, 115) + "...";
            }
            return PartialView(beers);
        }

        [ChildActionOnly]
        public ActionResult CoffeesList(string authorId)
        {
            // Grab three most recent coffee journals with authorId
            var coffees = db.Coffees.Include("Journal").Where(b => b.Journal.Author.Id == authorId).OrderBy(b => b.Journal.Date).Take(3).ToList();
            // TODO: Implement Journal.Description shortening
            return PartialView(coffees);
        }

        [ChildActionOnly]
        public ActionResult AddAsFriend(string friendID)
        {
            var friend = db.Users.Find(friendID);
            if(CheckFriendship(friend))
            {
                return PartialView(null);
            }
            else
            {
                return PartialView(friend);
            }
        }

        // TODO: create a nice view and query for all friends of the author
        public ActionResult AllFriends(string authorId)
        {
            Author author = db.Users.Find(authorId);
            // If profile isn't public, validate friendship before querying for their friends

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Returns true if friend and currently signed in user are friends
        private Boolean CheckFriendship(Author friend)
        {
            // Check friendship table with currently signed in user and requested user
            var you = db.Users.Find(User.Identity.GetUserId());

            var relationship = (from f in db.Friendships
                                where f.AuthorID1 == friend.Id
                                && f.AuthorID2 == you.Id
                                select f.Relation).FirstOrDefault();
            // true = friends, false = not friends
            return relationship;
        }
    }
}
