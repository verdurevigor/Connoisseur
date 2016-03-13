using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheConnoisseur.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage="You must enter a search term or phrase.")]
        [StringLength(50, MinimumLength=1, ErrorMessage="The search phrase must be between 1 and 50 characters.")]
        public string SearchTerm { get; set; }
        [Required(ErrorMessage="You must select the type of search.")]
        public string SearchType { get; set; }
    }
}