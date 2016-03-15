using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Beer
    {
        public int BeerID { get; set; }
        public Journal Journal { get; set; }
        [Range(0, 100, ErrorMessage="You really think the ABV isn't within that range?")]
        public decimal Abv { get; set; }
        [Range(0, 999, ErrorMessage = "You really think the IBU isn't within that range?")]
        public int Ibu { get; set; }
        [StringLength(30, ErrorMessage = "The season must be explained within 30 characters.")]
        public string Seasonal { get; set; }
        [StringLength(30, ErrorMessage = "The style must be explained within 30 characters.")]
        public string Style { get; set; }
    }
}