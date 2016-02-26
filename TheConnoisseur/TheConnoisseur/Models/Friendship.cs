using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Friendship
    {
        public virtual int FriendshipID { get; set; }
        public virtual int AuthorID1 { get; set; }
        public virtual int AuthorID2 { get; set; }
        public virtual string Relation { get; set; }
    }
}