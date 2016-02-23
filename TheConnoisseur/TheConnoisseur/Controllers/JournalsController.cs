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
        private TheConnoisseurContext db = new TheConnoisseurContext();
        
        // All Journal CRUD is done through the specific Journal type's controller

        // GET: Journals/Lists
        
        public ActionResult Lists()
        {/*      TODO: Create simple query to get short (3 items each) of each list type required for jlvm. Ask how to setup an automated query that updates a separate db table once an hour. Or try using List<ViewModelType> in the ListViewModel?
            JournalListsViewModel jlvm = new JournalListsViewModel()
            {
                BeerVMs = (from j in db.Journals
                           select new BeerViewModel()
                           {
                               Journal = j,
                               Beer = (from b in db.Beers
                                          where b.JournalID == j.JournalID
                                          select b).FirstOrDefault(),
                               Author = (from a in db.Authors
                                         where a.AuthorID == j.AuthorID
                                         select a).FirstOrDefault()
                           }).ToList(),

                CoffeeVMs = (from j in db.Journals
                             select new CoffeeViewModel() 
                             { 
                                 Journal = j,
                                 Coffee = (from c in db.Coffees
                                           where c.JournalID == j.JournalID
                                           select c).FirstOrDefault(),
                                 Author = (from a in db.Authors
                                          where a.AuthorID == j.AuthorID
                                          select a).FirstOrDefault()
                             }).ToList()
            };*/
            return View();
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
