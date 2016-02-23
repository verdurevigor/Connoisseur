using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class CoffeeViewModel
    {
        public Journal Journal { get; set; }
        public Author Author { get; set; }
        public Coffee Coffee { get; set; }
    }
}