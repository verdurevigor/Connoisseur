using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Beer
    {
        public virtual int BeerID { get; set; }
        public virtual int JournalID { get; set; }
        public virtual decimal Abv { get; set; }
        public virtual int Ibu { get; set; }
        public virtual string Seasonal { get; set; }
    }
}