using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheConnoisseur.Models;

namespace TheConnoisseur.Controllers
{
    // By default authentication is required
    // Authorization check is now added here.
    [Authorize(Roles = "Admin,Moderator")]
    public class AuthorizationController : Controller
    {
        // Private instance to access database
        private AppDbContext db = new AppDbContext();
        // Private instance of user manager gives access to identity control
        UserManager<Author> userManager = new UserManager<Author>(
            new UserStore<Author>(new AppDbContext()));

        // This action presents a page for navigating to the different types of authorized tasks - Roles and Moderating
        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }

        // Manage and Assign Roles
        #region
        // Get: Authorization/ManageRole
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            // prepopulate roles for the view's dropdown
            var list = PrePopulateRoleList();

            ViewBag.Roles = list;
            return View();
        }

        // GET: Authorization/ListRoles
        [Authorize(Roles = "Admin")]
        public ActionResult ListRoles()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        // GET: Authorization/DeleteRole?RoleName=
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRole(string RoleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (thisRole != null)
            { 
                db.Roles.Remove(thisRole);
                db.SaveChanges();
                return RedirectToAction("ListRoles");
            }
            return View("Error");
        }

        //
        // GET: /Roles/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult EditRole(string roleName)
        {
            var thisRole = db.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (thisRole != null)
            {
                return View(thisRole);
            }
            return View("Error");
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditRole(IdentityRole role)
        {
            try
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ListRoles");
            }
            catch
            {
                return RedirectToAction("ListRoles");
            }
        }

        // GET: Authorization/CreateRole
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRole()
        {
            return View();
        }

        //
        // POST: /Authorization/CreateRole
        // This is a new way of getting information back from a form! Look at the method parameter, and how collection variable is used.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateRole(FormCollection collection)
        {
            try
            {
                // Add a new role to IdentityRole using this initialization list
                // IdentityRole comes from Microsoft.AspNet.Identity.EntityFramework
                db.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                db.SaveChanges();
                ViewBag.ResultMessage = "Role " + collection["RoleName"] + " was added successfully.";
                return View();
            }
            catch
            {
                ViewBag.ResultMessage = "Failed to create role " + collection["RoleName"];
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Author user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = PrePopulateRoleList();

                ViewBag.Roles = list;
                ViewBag.username = user.UserName;
            }
            return View("ManageRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            Author user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully for " + UserName + "!";

            // prepopulat roles for the view dropdown
            var list = PrePopulateRoleList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }

        // POST: Authorization/DeleteRoleForUser/Username&RoleName
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            Author user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = RoleName + " removed from " + UserName + " successfully!";
            }
            else
            {
                ViewBag.ResultMessage = UserName + " doesn't belong to " + RoleName + ".";
            }
            // prepopulat roles for the view dropdown
            var list = PrePopulateRoleList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }
        #endregion

        // Delete Members
        #region

        // GET: Authorization/ManageMembers
        [Authorize(Roles = "Admin")]
        public ActionResult ManageMembers()
        {
            return View();
        }

        // POST: Authorization/SearchMembers/searchString
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SearchMembers(string nameSearch)
        {

            // Get members that matches the searchName. Be sure to conduct a case-insensitive search.
            var members = db.Users.ToList().Where(n => n.UserName.ToLower().Contains(nameSearch.ToLower()));

            //  Return the search name to display to user
            ViewBag.NameSeach = nameSearch;
            if (members.Count() == 0)
                ViewBag.ResultMessage = "No members found.";
            return View("ManageMembers", members);
        }

        //
        // GET: Authorization/DeleteMember/id
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteMember(string id)
        {
            try
            {
                var member = (from u in db.Users where u.Id == id select u).FirstOrDefault();
                //userManager.Delete(member);
                db.Users.Remove(member);
                ViewBag.ResultMessage = "Member " + member.UserName + " was successfully deleted.";
                db.SaveChanges();
                return View("ManageMembers");
            }
            catch
            {
                ViewBag.ResultMessage = "Member was not deleted.";
                return View("ManageMembers");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteMemberAndMessages(string id)
        {
            try
            {
                var member = (from u in db.Users where u.Id == id select u).FirstOrDefault();
                // Remove all beer journals
                var beers = db.Beers.Include("Journal.Author").Where(b => b.Journal.Author.Id == member.Id).ToList();
                foreach (Beer b in beers)
                {
                    db.Beers.Remove(b);
                }
                // Remove all journals
                var journals = db.Journals.Include("Author").Where(j => j.Author.Id == member.Id).ToList();
                foreach (Journal j in journals)
                {
                    db.Journals.Remove(j);
                }
                db.SaveChanges();
                // Remove all pending requests (should only be one, but use ToList just in case
                var pendings = db.FriendshipsPending.Where(p => p.AuthorID1 == member.Id || p.AuthorID2 == member.Id).ToList();
                foreach (FriendshipPending p in pendings)
                {
                    db.FriendshipsPending.Remove(p);
                }
                // Remove all friends
                var friendships1 = db.Friendships.Where(f => f.AuthorID1 == member.Id).ToList();
                foreach (Friendship f1 in friendships1)
                {
                    db.Friendships.Remove(f1);
                }
                var friendships2 = db.Friendships.Where(f => f.AuthorID2 == member.Id).ToList();
                foreach (Friendship f2 in friendships2)
                {
                    db.Friendships.Remove(f2);
                }
                db.SaveChanges();

                // Remove user
                db.Users.Remove(member);
                db.SaveChanges();
                ViewBag.ResultMessage = "Member " + member.UserName + " and all their related content was deleted.";
                return View("ManageMembers");
            }
            catch
            {
                ViewBag.ResultMessage = "Member and/or related content were not deleted.";
                return View("ManageMembers");
            }

        }

        #endregion

        // GET: Authorization/ManageBeer
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult ManageBeer()
        {
            return View();
        }

        // POST: Authorization/SearchBeer/searchString
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult SearchBeer(string searchTerm)
        {
            // Get list of messages where the body contains searchTerm, add to the Messages the associated Topic and Memebr
            var beers = db.Beers.Include("Journal.Author").Where(m => m.Journal.Description.Contains(searchTerm) || m.Journal.Title.Contains(searchTerm) || m.Journal.Title.Contains(searchTerm)).ToList();
            // Return the search term to display to user
            ViewBag.SearchTerm = searchTerm;
            return View("ManageBeer", beers);
        }

        //
        // GET: Authorization/DeleteBeer/id
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult DeleteBeer(int id)
        {
            try
            {
                Beer beer = db.Beers.Include("Journal").Where(b => b.BeerID == id).FirstOrDefault();
                Journal journal = db.Journals.Where(j => j.JournalID == beer.Journal.JournalID).FirstOrDefault();
                ViewBag.ResultMessage = "Beer journal entry '" + beer.Journal.Title + "' was successfully deleted.";
                db.Beers.Remove(beer);
                db.Journals.Remove(journal);

                db.SaveChanges();
                
                return View("ManageBeer");
            }
            catch
            {
                ViewBag.ResultMessage = "Beer journal entry was not deleted.";
                return View("ManageBeer");
            }
        }

        // Method is used to repopulate roles for the ManageRole view's dropdown
        private List<SelectListItem> PrePopulateRoleList()
        {

            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            return list;
        }
    }
}