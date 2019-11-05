using Microsoft.AspNetCore.Http;
using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tangy.Models.ViewModels
{
    public class Menuitem_ViewModel
    {
        public Menuitem menuitem { get; set; }

        public List<Category> categorylist { get; set; }
        public List<Subcategory>  Subcategorylist { get; set; }

        public IFormFile File { get; set; }

    }
}
