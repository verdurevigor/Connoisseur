using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Friendship
    {
        public virtual int FriendshipId { get; set; }
        public virtual string AuthorId1 { get; set; }
        public virtual string AuthorId2 { get; set; }
        public virtual string Relation { get; set; }
    }
}