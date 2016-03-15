using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Views
{
    public class JournalsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        
        // All Journal CRUD is done through the specific Journal type's controller

        // GET: Journals/Lists
        
        public ActionResult Lists()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult HighestBeerList()
        {
            // Get the ten highest rated beer reviews, only including public connoisseurs
            var beers = db.Beers.Include("Journal.Author").Where(b => b.Journal.Rating == 5 && b.Journal.Author.PrivacyType == 1).Take(10).ToList();
            return PartialView(beers);
        }

        [ChildActionOnly]
        public ActionResult LowestBeerList()
        {
            // Get the ten lowest rated beer reviews, only including public connoisseurs
            var beers = db.Beers.Include("Journal.Author").Where(b => b.Journal.Rating >= 1 && b.Journal.Rating <= 3 && b.Journal.Author.PrivacyType == 1).Take(10).ToList();
            return PartialView(beers);
        }

        // GET: Journals/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Journals/Search/searchTerm
        [HttpPost]
        public ActionResult Search(string searchTerm, string journalType)
        {
            // Get journals that match the searchTerm
            var journals = (from j in db.Journals
                            where j.Description.Contains(searchTerm) && j.PrivacyType == 1 //&& j.JType == journalType
                            select j).ToList();
            // Return searchTerm to display to user.
            ViewBag.SearchTerm = searchTerm;
            //ViewBag.JournalType = journalType;
            return View("Search", journals);
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
