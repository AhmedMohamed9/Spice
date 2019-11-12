using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrderDatails
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int orderid { get; set; }
        [ForeignKey("orderid")]
        public virtual OrderHeaders OrderHeaders { get; set; }
        [Required]
        public int Menuitemid { get; set; }
        [ForeignKey("Menuitemid")]
        public virtual Menuitem Menuitem { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public string Description { get; set; }
        [Required]
        public double price { get; set; }


    }
}
