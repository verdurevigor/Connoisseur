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
    
        /* Properties already present from AspNetUsers database table (object AppUser):
           Id, Country, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed
           TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName */
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