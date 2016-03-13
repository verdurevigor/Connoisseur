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
    public class BeersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Beers/BeerJournals
        public ActionResult BeerJournals()
        {
            // Get list of all beer journal entries for the currently signed in user
            var you = db.Users.Find(User.Identity.GetUserId());
            var beers = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == you.Id).ToList();
            
            return View(beers);
        }
        
        // GET: Beers/FriendBeerjournals/FriendId
        // Returns Author object to FriendBeerJournals page where ChildAction FriendsBeers is fired
        public ActionResult FriendBeerJournals(string friendID)
        {
            var friend = (from a in db.Users
                          where a.Id == friendID
                          select a).FirstOrDefault();
            // If user arrived with invalid parameter
            if (friend == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // In the event the author arrived to action of their own journal by search page
            if (friend.Id == User.Identity.GetUserId())
            {
                string yourId = User.Identity.GetUserId();
                var beer = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == yourId).ToList();
                return View("BeerJournals", beer);
            }
            // If profile is public, pass friend along
            if (friend.PrivacyType == 1)
            {
                return View(friend);
            }
            // Not public, check that friendship exists before gather their beer entries
            if (CheckFriendship(friend))
            {
                return View(friend);
            }
            // Private profile and not friends
            return RedirectToAction("PrivateProfile", "Authors");
        }

        [ChildActionOnly]
        public ActionResult BeersListFriend(string friendID)
        {
            var beers = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == friendID).ToList();
            return PartialView(beers);
        }



        [ChildActionOnly]
        public ActionResult FriendsBeers(string friendId)
        {
            // Query for all the friend's beer entries
            var beers = db.Beers.Include("Journal").Where(b => b.Journal.Author.Id == friendId).ToList();

            return PartialView(beers);
        }

        // GET: Beers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Journal,Abv,Ibu,Seasonal")] Beer beer, int rating, string glasstype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get currently user
                    Author current = db.Users.Find(User.Identity.GetUserId());

                    // Set input not directly entered on form or model
                    beer.Journal.Author = current;
                    beer.Journal.Date = DateTime.Now;
                    beer.Journal.ImagePath = "/Content/Images/beer" + glasstype + ".png";
                    beer.Journal.JType = 2;
                    beer.Journal.PrivacyType = current.PrivacyType;
                    beer.Journal.Rating = rating;
                    // Save data into database (both a Beer and Journal object will be saved)
                    db.Beers.Add(beer);
                    db.SaveChanges();
                    // Redirect to user's profile
                    return RedirectToAction("Index", "Authors");
                }
                catch
                {
                    // Failed to save in database
                    ViewBag.ResultMessage = "There was an error saving the entry. Please try again!";
                    return View(beer);
                }
            }// Invalid model
            return View(beer);
        }

        // GET: Beers/Edit/5
        public ActionResult Edit(int? beerID)
        {
            if (beerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Include("Journal.Author").Where(b => b.BeerID == beerID).FirstOrDefault();
            if (beer == null)
            {
                return HttpNotFound();
            }
            // Ensure original author is currently signed in user
            if (ValidateAuthor(beer.Journal))
            {
                return View(beer);
            }
            // Not original author
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeerID,Journal,Abv,Ibu,Seasonal,Style")] Beer form, int rating, string glasstype)
        {
            if (ModelState.IsValid)
            {
                // Pull original journal from database
                var original = db.Beers.Include("Journal").Where(b => b.BeerID == form.BeerID).FirstOrDefault();

                // Validate original author and ensure record from form and database match
                if (ValidateAuthor(original.Journal) && form.BeerID == original.BeerID && original.Journal.JournalID == form.Journal.JournalID)
                {
                    // Update original with form input
                    original.Journal.Description = form.Journal.Description;
                    original.Journal.ImagePath = "/Content/Images/beer" + glasstype + ".png";
                    original.Journal.Location = form.Journal.Location;
                    original.Journal.Maker = form.Journal.Maker;
                    original.Journal.Rating = rating;
                    original.Journal.Title = form.Journal.Title;
                    original.Abv = form.Abv;
                    original.Ibu = form.Ibu;
                    original.Seasonal = form.Seasonal;
                    original.Style = form.Style;

                    // Save data into database (both a Beer and Journal object will be saved)
                    db.Entry(original).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["ResultMessage"] = original.Journal.Title + " was successfully updated!";
                    // Redirect to user's beer journals
                    return RedirectToAction("BeerJournals");
                }
                // Not original author or mismatched IDs
                RedirectToAction("Index", "Home");  // TODO: send them somewhere other than their homepage, invalid nasty user! Or faulty db...
            }// Invalid model
            return View(form);
        }

        // GET: Beers/Delete/5
        public ActionResult Delete(int? journalID)
        {
            if (journalID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journal journal = db.Journals.Include("Author").Where(j => j.JournalID == journalID).FirstOrDefault();
            Beer beer = db.Beers.Where(b => b.Journal.JournalID == journal.JournalID).FirstOrDefault();
            // Invalid query token
            if (journal == null)
            {
                return HttpNotFound();
            }
            if (ValidateAuthor(journal))
            {
                // TODO: check that both journal and beer are deleted. Also get the result message to show on the view
                db.Beers.Remove(beer);
                db.Journals.Remove(journal);
                
                db.SaveChanges();
                TempData["ResultMessage"] = "The journal was deleted.";
                return RedirectToAction("BeerJournals");
            }
            // Current user did not write this journal, send to error page
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

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

        // Returns true if Journal object was created by currently signed in user
        private Boolean ValidateAuthor(Journal journal)
        {
            var you = db.Users.Find(User.Identity.GetUserId());
            if (journal.Author.Id == you.Id)
                return true;
            else
                return false;
        }
    }
}
