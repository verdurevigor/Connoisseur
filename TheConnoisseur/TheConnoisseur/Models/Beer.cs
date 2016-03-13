using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Beer
    {
        public int BeerID { get; set; }
        public Journal Journal { get; set; }
        public decimal Abv { get; set; }
        public int Ibu { get; set; }
        public string Seasonal { get; set; }
        public string Style { get; set; }
    }
}