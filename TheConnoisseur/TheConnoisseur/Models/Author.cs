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
        [StringLength(30, ErrorMessage="That first name might be real, but it is too long for usage on this site.")]
        public string FirstName { get; set; }
        [StringLength(30, ErrorMessage = "That last name might be real, but it is too long for usage on this site.")]
        public string LastName { get; set; }
        [StringLength(30, ErrorMessage = "We will only store 30 characters of that city's name.")]
        public string City { get; set; }
        [StringLength(2, ErrorMessage = "State must be a two letter abbreviation.")]
        public string State { get; set; }
        [Display(Name="Current Favorite")]
        [StringLength(50, ErrorMessage = "Your current favorite must be explained in at most 50 characters. One must be concise and broad.")]
        public string FavItem { get; set; }
        [StringLength(50, ErrorMessage = "A tagline is at most 50 characters. One must be concise and broad.")]
        public string Tagline { get; set; }
        [Display(Name = "Privacy Level")]
        [Range(1, 3)]
        public int PrivacyType { get; set; }
        public string AvatarPath { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime DateCreated { get; set; }
    }
}