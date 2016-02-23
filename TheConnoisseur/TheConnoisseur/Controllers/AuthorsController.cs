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
    public class AuthorsController : Controller
    {
        private TheConnoisseurContext db = new TheConnoisseurContext();

        // GET: Authors
        public ActionResult Index()
        {

            // TODO: Get currently logged in user and pass their author information to the view
            
            // Currently testing view with a hard coded author
            Author author = (from a in db.Authors
                        where a.AuthorID == 1
                        select a).FirstOrDefault();
            return View(author);
        }

        // TODO: use this method when a friend's profile link is clicked.
        // GET: Authors/FriendProfile/1
        public ActionResult FriendProfile(int friendID)
        {
            Author author = (from a in db.Authors
                             where a.AuthorID == friendID
                             select a).FirstOrDefault();
            // TODO: ensure that author found has "friend" relation with current identity
            return View("Index", author);
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
