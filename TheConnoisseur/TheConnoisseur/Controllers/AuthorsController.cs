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

        public ActionResult PrivateProfile(Author author)
        {
            if (author != null)
            { 
                return View("PrivateProfile", author);
            }
            return View("Error");
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
                return View("Error");
            }
            // Your own profile
            if (friend.Id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index");
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
            // AuthorID2 is you and AuthorID1 is the friend
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
            var beers = db.Beers.Include("Journal.Author").Where(b => b.Journal.Author.Id == authorId).OrderBy(b => b.Journal.Date).Take(3).ToList();
            
            // Shorten the description for brief viewing
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

        // TODO: create a nice view and query for all friends of the author
        // This view will have a "remove" button added to each profile. Return author and have childaction get list of friend authors, attaching the remove button to each profile
        public ActionResult AllYourFriends()
        {
            // Get currently signed in user and gather their friends Ids from the friendship table, then get the Author objects with that.
            string yourID = User.Identity.GetUserId();
            // AuthorID2 is you and AuthorID1 is the friend
            var friendIds = (from a in db.Users
                           join f in db.Friendships on a.Id equals f.AuthorID1
                           where f.AuthorID2 == yourID && f.Relation == true
                           select f.AuthorID1).ToList();
            List<Author> friends = new List<Author>();
            foreach (var f in friendIds)
            {
                Author a = db.Users.Find(f);
                friends.Add(a);
            }

            return View("AllYourFriends", friends);
        }

        // Validate friendship, then pass the friend author into the view, use childaction to get list of their friends, use another child action to load add friend button for each author iterated in list.
        public ActionResult AllTheirFriends(string authorID)
        {
            var friend = db.Users.Find(authorID);
            // User arrived with invalid parameter
            if (friend == null)
            {
                return View("Error");
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
        public ActionResult AllTheirFriendsList(string authorID)
        {
            // AuthorID2 is 'you' and AuthorID1 is their friend
            var friendIds = (from f in db.Friendships
                             where f.AuthorID2 == authorID && f.Relation == true
                             select f.AuthorID1).ToList();
            List<Author> friends = new List<Author>();
            foreach (var f in friendIds)
            {
                Author a = db.Users.Find(f);
                friends.Add(a);
            }
            return PartialView(friends);
        }

        [ChildActionOnly]
        public ActionResult AddNewFriendButton(string friendID)
        {
            var friend = db.Users.Find(friendID);
            // it's you
            if (friend.Id == User.Identity.GetUserId())
            {
                return null;
            }
            // check if already friends
            if (CheckFriendship(friend) == false)
            {
                string yourID = User.Identity.GetUserId();
                // Ensure you haven't already made a request
                var pending1 = db.FriendshipsPending.Where(p => p.AuthorID1 == friendID && p.AuthorID2 == yourID).FirstOrDefault();
                
                if (pending1 == null)
                {   
                    // You haven't made a request
                    // Now ensure they haven't made a request
                    var pending2 = db.FriendshipsPending.Where(p => p.AuthorID1 == yourID && p.AuthorID2 == friendID).FirstOrDefault();
                    
                    if (pending2 == null)
                    {   
                        // They haven't made a request
                        // Display add friend button
                        return PartialView(friend);
                    }
                }
            }
            // Already friends or pending request, don't load friend button
            return null;
        }

        // This Action renders an Accept Friend button on FriendProfile or any other view if there is a pending request
        [ChildActionOnly]
        public ActionResult AcceptNewFriendButton(string friendID)
        {
            var them = db.Users.Find(friendID);
            var you = db.Users.Find(User.Identity.GetUserId());
            // it's you
            if (them.Id == you.Id)
            {
                return null;
            }
            // Check if already friends
            if (CheckFriendship(them) == false)
            {
                // Not friends
                // Check if they they requested friendship
                var pending = db.FriendshipsPending.Where(p => p.AuthorID1 == you.Id && p.AuthorID2 == them.Id).FirstOrDefault();
                
                if (pending != null)
                {   
                    // Friendship request exists
                    return PartialView(them);
                }
            }
            // Already friends or no request, don't load button
            return null;
        }

        public ActionResult RequestFriendship(string friendID)
        {
            // Validate query token
            var friend = db.Users.Find(friendID);
            if (friend != null)
            {
                // Ensure a request hasn't already been placed.
                string yourId = User.Identity.GetUserId();
                var exists = db.FriendshipsPending.Where(f => f.AuthorID1 == friend.Id && f.AuthorID2 == yourId).FirstOrDefault();
                if (exists == null) // Request doesn't exist
                {
                    FriendshipPending fp = new FriendshipPending() { AuthorID1 = friend.Id, AuthorID2 = yourId };
                    db.FriendshipsPending.Add(fp);
                    db.SaveChanges();
                    // Request made, send to friend's profile with BefriendResultMessage
                    TempData["BefriendResultMessage"] = "A friend request has been made.";
                    return RedirectToAction("FriendProfile", new { friendID = friend.Id });// Action not view...
                }
                // Subsequent request placed, inform user.
                TempData["BefriendResultMessage"] = "You have already requested a friendship.";
                return RedirectToAction("FriendProfile", "Authors", new { friendID = friend.Id });
            }
            // Invalid request.
            return View("Error");
        }

        public ActionResult PendingFriends()
        {
            string yourID = User.Identity.GetUserId();
            // If they requested a friendship then AuthorID1 is them, you are AuthorID2
            var theirIDs = (from p in db.FriendshipsPending where p.AuthorID1 == yourID select p.AuthorID2).ToList();
            // Get list of authors from requester Ids list.
            List<Author> requesters = new List<Author>();
            foreach (var id in theirIDs)
            {
                Author r = db.Users.Find(id);
                requesters.Add(r);
            }
            // Populate PendingFriends view with list of requesting Authors
            return View(requesters);
        }

        [HttpPost]
        public ActionResult AcceptFriend(string friendID)
        {
            string yourID = User.Identity.GetUserId();
            // Gather pending request from database
            // If they requested a friendship then AuthorID1 is you and AuthorID2 is them
            var pending = db.FriendshipsPending.Where(p => p.AuthorID1 == yourID && p.AuthorID2 == friendID).FirstOrDefault();
            if (pending != null)
            {
                // Add two Friendship records to database, remove FriendshipPending record
                Friendship f1 = new Friendship() { AuthorID1 = pending.AuthorID1, AuthorID2 = pending.AuthorID2, Relation = true };
                Friendship f2 = new Friendship() { AuthorID1 = pending.AuthorID2, AuthorID2 = pending.AuthorID1, Relation = true };
                db.Friendships.Add(f1);
                db.Friendships.Add(f2);
                db.FriendshipsPending.Remove(pending);
                db.SaveChanges();
                // Redirect you to PendingFriends action with message
                string friendUsername = db.Users.Where(u => u.Id == f1.AuthorID2).Select(u => u.UserName).FirstOrDefault();
                TempData["ResultMessage"] = friendUsername + " is now your connoisseur friend!";
                return RedirectToAction("PendingFriends");
            }

            return View("Error");
        }

        [HttpPost]
        public ActionResult RemoveFriend(string friendID)
        {
            string yourID = User.Identity.GetUserId();
            // Gather friendship from database
            var removing1 = db.Friendships.Where(f => f.AuthorID1 == friendID && f.AuthorID2 == yourID && f.Relation == true).FirstOrDefault();
            if (removing1 != null)
            {
                var removing2 = db.Friendships.Where(f => f.AuthorID2 == friendID && f.AuthorID1 == yourID && f.Relation == true).FirstOrDefault();
                // Remove Friendship record from database
                db.Friendships.Remove(removing1);
                db.Friendships.Remove(removing2);
                db.SaveChanges();
                // Redirect you to AllYourFriends action with result message
                string removedUsername = db.Users.Where(u => u.Id == friendID).Select(u => u.UserName).FirstOrDefault();
                TempData["ResultMessage"] = removedUsername + " has been removed from your friends list.";
                return RedirectToAction("AllYourFriends");
            }

            return View("Error");
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
            // AuthorID2 is you, AuthorID1 is the friend
            var relationship = (from f in db.Friendships
                                where f.AuthorID1 == friend.Id && f.AuthorID2 == you.Id && f.Relation == true
                                select f.Relation).FirstOrDefault();
            // true = friends, null = not friends
            if (relationship == true)
                return true;
            else
                return false;
        }
    }
}
