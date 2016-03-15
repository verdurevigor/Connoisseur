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
        [StringLength(30, ErrorMessage = "The Maker must be explained within 30 characters.")]
        public string Maker { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "The title might be long, but it must be explained within 30 characters.")]
        public string Title { get; set; }
        public int JType { get; set; }   // Int represents: 1 (coffee), 2 (beer)
        public string ImagePath { get; set; }
        // Range of 1-5, not implementing annotation due to radio buttons lack of ability to hold the value of an int...
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "While this is a journal, you get one 'page' for each entry. Please write less than 1000 characters")]
        public string Description { get; set; }      // TODO: set max character limit.
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        [StringLength(30, ErrorMessage = "The Location might be large, but it must be explained within 30 characters.")]
        public string Location { get; set; }
        public int PrivacyType { get; set; }     // Int represents: 1 (public), 3 (friends only) - based on use's privacy setting

    }
}