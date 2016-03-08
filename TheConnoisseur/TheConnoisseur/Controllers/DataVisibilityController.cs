﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;
using Microsoft.AspNet.Identity;

namespace TheConnoisseur.Controllers
{
    [AllowAnonymous]
    public class DataVisibilityController : Controller
    {
        AppDbContext db = new AppDbContext();

        // GET: DataVisibility
        public ActionResult Index()
        {
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
    }
}