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
            // TODO: query database for a short list of jounals
            // For now this is used to display all journals and delete them (testing purposes)
            var journals = db.Journals.ToList();

            return View(journals);
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
