using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.Viewmodel
{
    public class Index_ViewModel
    {
        public IEnumerable<Menuitem> Menuitems { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Copuns> Copuns { get; set; }

    }
}
