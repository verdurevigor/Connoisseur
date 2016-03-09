using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class Journal
    {
        public int JournalID { get; set; }
        public Author Author { get; set; }
        [Required]
        public string Maker { get; set; }
        [Required]
        public string Title { get; set; }
        public int JType { get; set; }   // Int represents: 1 (coffee), 2 (beer)
        public string ImagePath { get; set; }
        // Range of 1-5, not implementing annotation due to radio buttons lack of ability to hold the value of an int...
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Description { get; set; }      // TODO: set max character limit.
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int PrivacyType { get; set; }     // Int represents: 1 (public), 3 (friends only) - based on use's privacy setting

    }
}