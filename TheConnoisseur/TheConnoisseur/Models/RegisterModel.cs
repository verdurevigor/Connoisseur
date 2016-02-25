using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        // TODO: update RegisterModel to take all necessary/optional user info
        public virtual string City { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "State must be a two letter abbreviation.")]
        public virtual string State { get; set; }
    }
}