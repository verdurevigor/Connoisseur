using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Author : IdentityUser
    {
        /* IdentityUser already has these properties
         * Email, EmailConfirmed, Id, PasswordHash, PhoneNumber, PhoneNumberConfirmed, UserName
         * Some other properties which offer navigation to more things are Claims and Roles.
         * */
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name="Current Favorite")]
        public string FavItem { get; set; }
        public string Tagline { get; set; }
        public int PrivacyType { get; set; }
        public string AvatarPath { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime DateCreated { get; set; }
    }
}