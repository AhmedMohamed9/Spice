using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
       
    }
}
