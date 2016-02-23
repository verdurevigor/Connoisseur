using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class JournalListsViewModel
    {
        public IEnumerable<BeerViewModel> BeerVMs { get; set; }
        public IEnumerable<CoffeeViewModel> CoffeeVMs { get; set; }
    }
}