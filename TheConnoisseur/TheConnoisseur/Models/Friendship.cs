using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Friendship
    {
        public int FriendshipID { get; set; }
        public string AuthorID1 { get; set; }
        public string AuthorID2 { get; set; }
        public bool Relation { get; set; }  // true == friends, false == blocked
    }
}