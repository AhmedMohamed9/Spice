using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.Viewmodel
{
    public class SubcategoryViewModel
    {
        public Subcategory subCategory { get; set; }

        //public int categoryid { get; set; }

        public List<Category> categorieslist { get; set; }

        public List<string> subcategorylist { get; set; }

        [Display(Name = "New Sub Category")]
        public bool isnew { get; set; }

        public string StatusMessage { get; set; }
    }
}
