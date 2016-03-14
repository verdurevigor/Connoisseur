using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class FriendshipPending
    {
        public int FriendshipPendingID { get; set; }
        public string AuthorID1 { get; set; }
        public string AuthorID2 { get; set; }
    }
}