using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class RegisterViewModel
    {
        // TODO: Annotate error messages.
        [Required]
        [StringLength(16, MinimumLength=2)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage="Email address is too long.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength=8, ErrorMessage="Your password consist of 8 to 16 characters.")]
        public string Password { get; set; }
        [Compare("Password")]   
        [DataType(DataType.Password)]
        [Display(Name = "Privacy Type")]
        [Required]
        public string PasswordConfirmed { get; set; }
        [StringLength(30, ErrorMessage = "We will only store 30 characters of that city's name.")]
        public virtual string City { get; set; }
        [StringLength(2, ErrorMessage = "State must be a two letter abbreviation.")]
        public virtual string State { get; set; }
        [Display(Name="Privacy Level")]
        [Range(1,3)]
        public int PrivacyType { get; set; }
    }
}