using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Copuns
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        public string CouponType { get; set; }
        public enum EcouponType { Percent = 0, Dollar = 1 }
        [Required]

        public double Discount { get; set; }

        [Required]

        public double MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }
    }
}
