using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Coffee
    {
        public virtual int CoffeeID { get; set; }
        public virtual int JournalID { get; set; }
        public virtual string RoastType { get; set; }
        public virtual string Origin { get; set; }
        public virtual string FoodPairing { get; set; }
    }
}