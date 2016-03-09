using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace TheConnoisseur.Controllers
{
    [AllowAnonymous]
    public class DataVisibilityController : Controller
    {
        AppDbContext db = new AppDbContext();

        // GET: DataVisibility
        public ActionResult Index()
        {
            Beer beer = db.Beers.Include("Journal").Where(b => b.Journal.Maker == "Good Life").FirstOrDefault();
            db.Journals.Remove(beer.Journal);
            db.Beers.Remove(beer);
            db.SaveChanges();
            return View();
        }

        [ChildActionOnly]
        public ActionResult Authors()
        {
            var authors = db.Users.ToList();
            return PartialView(authors);
        }

        [ChildActionOnly]
        public ActionResult Beers()
        {
            var beers = db.Beers.Include("Journal.Author").ToList();
            return PartialView(beers);
        }

        [ChildActionOnly]
        public ActionResult Coffees()
        {
            var coffees = db.Coffees.Include("Journal.Author").ToList();
            return PartialView(coffees);
        }

        public ActionResult UpdateBeerIcons()
        {
            var beers = db.Beers.Include("Journal").ToList();
            foreach (Beer b in beers)
            {
                b.Journal.ImagePath = "/Content/Images/beerglass.png";
                db.Entry(b).State = EntityState.Modified;
            }

            db.SaveChanges();
            return View("Index");
        }
        public ActionResult UpdateCoffeeIcons()
        {
            var coffees = db.Coffees.Include("Journal").ToList();
            foreach (Coffee c in coffees)
            {
                c.Journal.ImagePath = "/Content/Images/coffeemug.png";
                db.Entry(c).State = EntityState.Modified;
            }

            db.SaveChanges();
            return View("Index");
        }
    }
}