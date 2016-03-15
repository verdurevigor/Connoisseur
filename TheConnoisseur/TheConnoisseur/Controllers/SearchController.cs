using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Controllers
{
    public class SearchController : Controller
    {
        AppDbContext db = new AppDbContext();


        // Types of searches available
        private List<string> types = new List<string>()
        {
            //"Coffee", 
            "Beer", 
            "Author"
        };

        // GET: Search
        public ActionResult Search()
        {
            // Populate dropdownlist
            ViewBag.Types = PrePopulateTypeList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include="SearchTerm,SearchType")]SearchViewModel svm)
        {
            if (ModelState.IsValid)
            {
                // Populate dropdownlist
                ViewBag.Types = PrePopulateTypeList();
                // Set view model for server logic to determine which partial view to render
                return View(svm);
            }
            // Not valid
            // Populate dropdownlist
            ViewBag.Types = PrePopulateTypeList();
            return View(svm);
        }
        
        [ChildActionOnly]
        public ActionResult ResultBeer(string searchTerm)
        {
            // Returns a list of beers by querying against Description, Title, Maker, and Style
            var beers = db.Beers.Include("Journal.Author").Where(b => b.Journal.PrivacyType == 1 && b.Journal.Description.Contains(searchTerm) || b.Journal.Title.Contains(searchTerm) || b.Journal.Maker.Contains(searchTerm) || b.Journal.Location.Contains(searchTerm) || b.Style.Contains(searchTerm)).ToList();
            return PartialView(beers);
        }

        [ChildActionOnly]
        public ActionResult ResultAuthor(string searchTerm)
        {
            // Returns a list of authors by querying against UserName, First and Last Names, Last name, First name, and City
            var authors = db.Users.Where(a => a.UserName.Contains(searchTerm) || (a.FirstName.Contains(searchTerm) && a.LastName.Contains(searchTerm)) || a.LastName.Contains(searchTerm) || a.FirstName.Contains(searchTerm) || a.City.Contains(searchTerm)).ToList();
            return PartialView(authors);
        }
        
        // Method is used to repopulate search type
        private List<SelectListItem> PrePopulateTypeList()
        {
            // Create the List<SelectListItem> setting values and texts to the same strings as private instance variable
            var list = types.ToList().Select(rr =>
            new SelectListItem { Value = rr, Text = rr }).ToList();

            return list;
        }
    }
}