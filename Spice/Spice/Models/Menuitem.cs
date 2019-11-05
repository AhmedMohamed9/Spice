using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Menuitem
    { 
        public int id { get; set; }
        [Required]
        public string name { get; set; }

        public string image { get; set; }

        public string description { get; set; }

        public string spiceness { get; set; }
        public enum Espice { Na = 0, Spicy = 1, Veryspicy = 2 }

        [Range(1, int.MaxValue, ErrorMessage = "Price Must be more than ${1}")]
        public double Price { get; set; }

        [Display(Name = "Category")]
        public int categoryid { get; set; }

        [ForeignKey("categoryid")]
        public virtual Category Category { get; set; }

        [Display(Name = "SubCategory")]
        public int subcategoryid { get; set; }

        [ForeignKey("subcategoryid")]
        public virtual Subcategory subcategory { get; set; }
    }
}
