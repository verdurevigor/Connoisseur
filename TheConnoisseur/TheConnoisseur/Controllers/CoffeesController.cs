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
    public class CoffeesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Coffees
        public ActionResult Index()
        {
            return View(db.Coffees.ToList());
        }

        // GET: Coffees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Using JournalID get Coffee object get full object
            Coffee coffee = db.Coffees.Include("Journal").Include("Author").Where(c => c.Journal.JournalID == id).FirstOrDefault();

            if (coffee == null)
            {
                return HttpNotFound();
            }

            return View(coffee);
        }

        // GET: Coffees/Create
        public ActionResult Create()
        {
            ViewBag.PrivacyList = GetPrivacyList(null);
            return View();
        }

        // POST: Coffees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Journal,Coffee")] Coffee form, int PrivacyList)
        { 
            if (ModelState.IsValid)
            {
                // Create journal object and add to database
                Journal j = new Journal()
                {
                    Author = db.Users.Find(User.Identity.GetUserId()),
                    Date = DateTime.Now,
                    Description = form.Journal.Description,
                    JType = 1,
                    Location = form.Journal.Location,
                    Maker = form.Journal.Maker,
                    PrivacyType = PrivacyList,
                    Rating = form.Journal.Rating,
                    Title = form.Journal.Title
                };
                db.Journals.Add(j);
                db.SaveChanges();

                // Create pairing coffee object and add to database
                Coffee c = new Coffee()
                {
                    FoodPairing = form.FoodPairing,
                    Journal = j,
                    Origin = form.Origin,
                    RoastType = form.RoastType
                };

                db.Coffees.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");   // TODO: make sure this links to appropriate page. Consider the user's profile.
            }
            ViewBag.PrivacyList = GetPrivacyList(null);
            return View(form);
        }

        // GET: Coffees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get coffee object using FK JournalID
            Coffee coffee = db.Coffees.Include("Journal").Include("Author").Where(c => c.Journal.JournalID == id).FirstOrDefault();
            if (coffee == null)
            {
                return HttpNotFound();
            }
            // Set dropdownlist to preselected privacy type.
            ViewBag.PrivacyList = GetPrivacyList(coffee.Journal.PrivacyType);
            return View(coffee);
        }

        // POST: Coffees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Journal,Author,Coffee")] Coffee form, int PrivacyList)
        {
            if (ModelState.IsValid)
            {
                // Pull Coffee journal from database, check authorization (same user), update, and save
                Coffee coffee = db.Coffees.Include("Journal").Include("Author").Where(c => c.Journal.JournalID == form.CoffeeID).FirstOrDefault();
                if (coffee.Journal.Author != db.Users.Find(User.Identity.GetUserId()))
                {
                    RedirectToAction("Index", "Home");  // TODO: send invalid editor somewhere worse than homepage
                }
                coffee.Journal.Description = form.Journal.Description;
                coffee.Journal.ImagePath = form.Journal.ImagePath;
                coffee.Journal.Location = form.Journal.Location;
                coffee.Journal.Rating = form.Journal.Rating;
                coffee.Journal.Title = form.Journal.Title;
                coffee.Journal.PrivacyType = PrivacyList;


                coffee.FoodPairing = form.FoodPairing;
                coffee.Origin = form.Origin;
                coffee.RoastType = form.RoastType;
 
                
                // Set objects and save
                db.Entry(coffee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PrivacyList = GetPrivacyList(PrivacyList);
            return View(form);
        }

        // GET: Coffees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get coffee object using FK JournalID
            Coffee coffee = db.Coffees.Include("Journal").Where(c => c.Journal.JournalID == id).FirstOrDefault();
            if (coffee == null)
            {
                return HttpNotFound();
            }
            return View(coffee);
        }

        // POST: Coffees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // The id being passed is the JounalID
            Journal journal = db.Journals.Find(id);
            // Get coffee object using FK JournalID
            Coffee coffee = db.Coffees.Where(c => c.Journal.JournalID == journal.JournalID).FirstOrDefault();
            
            db.Coffees.Remove(coffee);
            db.Journals.Remove(journal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList GetPrivacyList(int? current)
        {
            SelectList s1;
            if (current == null)
            {
                s1 = new SelectList(db.Privacies.OrderBy(p => p.Name), "PrivacyID", "Name", 1);
                return s1;
            }
            s1 = new SelectList(db.Privacies.OrderBy(p => p.Name), "PrivacyID", "Name", current);
            return s1;
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
