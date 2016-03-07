using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Coffee
    {
        public int CoffeeID { get; set; }
        public Journal Journal { get; set; }
        public string RoastType { get; set; }
        public string Origin { get; set; }
        public string FoodPairing { get; set; }
    }
}