using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Author
    {
        public virtual int AuthorID { get; set; }
        [Display(Name="Username")]
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string EmailConfirm { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordConfirm { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        [Display(Name="Current Favorite")]
        public virtual string FavItem { get; set; }
        public virtual string Tagline { get; set; }
        public virtual int PrivacyType { get; set; }
        public virtual string AvatarPath { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public virtual DateTime DateCreated { get; set; }
    }
}