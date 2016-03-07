using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Privacy
    {
        public int PrivacyID { get; set; }
        public string Name { get; set; }    // 1 Public, 2 Private, 3 FriendsOnly
    }
}