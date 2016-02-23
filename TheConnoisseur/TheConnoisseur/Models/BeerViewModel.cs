using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class BeerViewModel
    {
        public Journal Journal { get; set; }
        public Author Author { get; set; }
        public Beer Beer { get; set; }
    }
}