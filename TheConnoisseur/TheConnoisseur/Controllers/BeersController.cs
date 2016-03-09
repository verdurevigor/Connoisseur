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

        // GET: Beers
        public ActionResult Index()
        {
            return View(db.Beers.ToList());
        }

        // GET: Beers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
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
        public ActionResult Create([Bind(Include = "BeerID,Journal,Abv,Ibu,Seasonal")] Beer beer, int rating, string glasstype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create Jounal and save it to db, then create Beer and save to database!
                    // Get currently user
                    Author current = db.Users.Find(User.Identity.GetUserId());
                    /*
                    Journal j = new Journal()
                    {
                        Author = current,
                        Date = new DateTime(),
                        Description = beer.Journal.Description,
                        ImagePath = "/Content/Images/beer" + glasstype + ".jpg",
                        JType = 2,
                        Location = beer.Journal.Location,
                        Maker = beer.Journal.Maker,
                        PrivacyType = current.PrivacyType,
                        Rating = rating,
                        Title = beer.Journal.Title
                    };*/

                    // Set input not directly entered on form
                    beer.Journal.Author = current;
                    beer.Journal.Date = DateTime.Now;
                    beer.Journal.ImagePath = "/Content/Images/beer" + glasstype + ".png";
                    beer.Journal.JType = 2;
                    beer.Journal.PrivacyType = current.PrivacyType;
                    beer.Journal.Rating = rating;
                    // Save data into database
                    db.Beers.Add(beer);
                    db.SaveChanges();
                    // Redirect to user profile
                    return RedirectToAction("Index", "Authors");
                }
                catch
                {
                    // Failed to save in database
                    return View(beer);
                }
            }// Invalid model
            return View(beer);
        }

        // GET: Beers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeerID,JournalID,Abv,Ibu,Seasonal")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beer);
        }

        // GET: Beers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beer beer = db.Beers.Find(id);
            db.Beers.Remove(beer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
