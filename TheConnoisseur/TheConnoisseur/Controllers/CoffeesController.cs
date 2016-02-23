using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Controllers
{
    public class CoffeesController : Controller
    {
        private TheConnoisseurContext db = new TheConnoisseurContext();

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
            // Get coffee object using FK JournalID sent from ActionLink
            Coffee coffee = (from c in db.Coffees
                             where c.JournalID == id
                             select c).FirstOrDefault();
            if (coffee == null)
            {
                return HttpNotFound();
            }

            Journal journal = db.Journals.Find(coffee.JournalID);

            if (journal == null)
            {
                return HttpNotFound();
            }
            // Journal record exists, make ViewModel and pass to edit view
            CoffeeViewModel cvm = new CoffeeViewModel()
            {
                Author = (from a in db.Authors
                          where a.AuthorID == journal.AuthorID
                          select a).FirstOrDefault(),
                Journal = journal,
                Coffee = coffee
            };
            return View(cvm);
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
        public ActionResult Create([Bind(Include = "Journal,Coffee")] CoffeeViewModel cvm, int PrivacyList)
        { 
            if (ModelState.IsValid)
            {
                // Create journal object and add to database
                Journal j = new Journal()
                {
                    AuthorID = 1,   // TODO: use Identity to set this property
                    Date = DateTime.Now,
                    Description = cvm.Journal.Description,
                    JType = "co",
                    Location = cvm.Journal.Location,
                    Maker = cvm.Journal.Maker,
                    PrivacyType = PrivacyList,
                    Rating = cvm.Journal.Rating,
                    Title = cvm.Journal.Title
                };
                db.Journals.Add(j);
                db.SaveChanges();

                // Create pairing coffee object and add to database
                Coffee c = new Coffee()
                {
                    FoodPairing = cvm.Coffee.FoodPairing,
                    JournalID = j.JournalID,
                    Origin = cvm.Coffee.Origin,
                    RoastType = cvm.Coffee.RoastType
                };

                db.Coffees.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");   // TODO: make sure this links to appropriate page. Consider the user's profile.
            }
            ViewBag.PrivacyList = GetPrivacyList(null);
            return View(cvm);
        }

        // GET: Coffees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get coffee object using FK JournalID
            Coffee coffee = (from c in db.Coffees
                             where c.JournalID == id
                             select c).FirstOrDefault();
            if (coffee == null)
            {
                return HttpNotFound();
            }
            Journal journal = db.Journals.Find(coffee.JournalID);
            if (journal == null)
            {
                return HttpNotFound();
            }
            // Journal record exists, make ViewModel and pass to edit view
            CoffeeViewModel cvm = new CoffeeViewModel()
            {
                Author = (from a in db.Authors
                          where a.AuthorID == journal.AuthorID
                          select a).FirstOrDefault(),
                Journal = journal,
                Coffee = coffee
            };
            // Set dropdownlist to preselected privacy type.
            ViewBag.PrivacyList = GetPrivacyList(journal.PrivacyType);
            return View(cvm);
        }

        // POST: Coffees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Journal,Author,Coffee")] CoffeeViewModel cvm, int PrivacyList)
        {
            if (ModelState.IsValid)
            {
                // Pull Journal from database, check authorization (same user), update, and save
                Journal journal = (from j in db.Journals
                             where j.JournalID == cvm.Journal.JournalID
                             select j).FirstOrDefault();
                if (journal.AuthorID == 1 || journal.AuthorID == 2)    // TODO: Update this if statement to check authorid against the current identity
                {
                    // TODO: send user to login page, or a "your kicked out" page
                    RedirectToAction("Index", "Home");
                }
                journal.Description = cvm.Journal.Description;
                journal.ImagePath = cvm.Journal.ImagePath;
                journal.Location = cvm.Journal.Location;
                journal.Rating = cvm.Journal.Rating;
                journal.Title = cvm.Journal.Title;
                journal.PrivacyType = PrivacyList;

                // Pull Coffee from database, update fields, and save
                Coffee coffee = (from c in db.Coffees
                            where c.JournalID == journal.JournalID
                            select c).FirstOrDefault();

                coffee.FoodPairing = cvm.Coffee.FoodPairing;
                coffee.Origin = cvm.Coffee.Origin;
                coffee.RoastType = cvm.Coffee.RoastType;
 
                
                // Set objects and save
                db.Entry(journal).State = EntityState.Modified;
                db.Entry(coffee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PrivacyList = GetPrivacyList(PrivacyList);
            return View(cvm);
        }

        // GET: Coffees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get coffee object using FK JournalID
            Coffee coffee = (from c in db.Coffees
                             where c.JournalID == id
                             select c).FirstOrDefault();
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
            Coffee coffee = (from c in db.Coffees
                             where c.JournalID == journal.JournalID
                             select c).FirstOrDefault();
            
            db.Coffees.Remove(coffee);
            db.Journals.Remove(journal);
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
