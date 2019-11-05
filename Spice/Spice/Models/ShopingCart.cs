using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{ 
    public class ShopingCart
    {
        public ShopingCart()
        {
            count = 1;
        }
        public int id { get; set; }
        public string applicationuserid { get; set; }

        [NotMapped]
        [ForeignKey("applicationuserid")]
        public ApplicationUser applicationUser { get; set; }

        public int menuitemid { get; set; }
        [NotMapped]
        [ForeignKey("menuitemid")]
        public Menuitem Menuitem { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Enter Value more than 0")]
        public int count { get; set; }
    }
}
