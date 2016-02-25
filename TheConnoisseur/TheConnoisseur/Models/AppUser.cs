using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    // Use this class to store addtional information about the users/authors
    public class AppUser : IdentityUser
    {
        // TODO: this property is from the blog example, remove it once complete with blog Identity
        public string Country { get; set; }

        // TODO: testing to add Author info here, implement more once it works
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        [Display(Name = "Current Favorite")]
        public virtual string FavItem { get; set; }
        public virtual string Tagline { get; set; }
    }
}