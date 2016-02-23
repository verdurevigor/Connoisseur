using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Controllers
{
    // This private controller inherits from AppController to gain access to
    // the current user.
    public class AdminController : AppController
    {
        // Instance of database context to use in queries
        private TheConnoisseurContext db = new TheConnoisseurContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Journals/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: Journals/Search/searchTerm
        [HttpPost]
        public ActionResult Search(string searchTerm, string searchType)
        {
            if (searchType == "be")
            {
                // Get journals that match the searchTerm
                var beers = (from j in db.Journals
                             where j.Description.Contains(searchTerm)
                             && j.JType == searchType
                             select j).ToList();
                // Return searchTerm to display to user.
                ViewBag.SearchTerm = searchTerm;
                return View("Search", beers);
            }
            else if (searchType == "co")
            {
                var coffees = (from j in db.Journals
                               where j.Description.Contains(searchTerm)
                               && j.JType == searchType
                               select j).ToList();
                ViewBag.SearchTerm = searchTerm;
                return View("Search", coffees);
            }
            else if (searchType == "author")
            {
                var authors = (from m in db.Authors
                               where m.Username == searchTerm || m.FirstName == searchTerm
                               select m).ToList();
                ViewBag.SearchTerm = searchTerm;
                return View("Search", authors);
            }
            else
                return View("Search");
        }
    }
}