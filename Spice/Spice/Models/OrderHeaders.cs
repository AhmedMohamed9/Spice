using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrderHeaders
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string userid { get; set; }
        [ForeignKey("userid")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime orderdate { get; set; }
        [Required]
        public double OrdartotalOriginal { get; set; }
        [Required]
        [Display(Name ="Order Total")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public double ordertotal { get; set; }
        [Required]
        [Display(Name ="Pick UP Time")]
        public DateTime Picktime { get; set; }
        [Required]
        [NotMapped]
        [Display(Name ="Pick UP Date")]
        public DateTime PickDate { get; set; }
        [NotMapped]
        public List<Copuns> copunslist { get; set; }
        [Display(Name ="Coupon Code")]
    public string CouponCode { get; set; }
        public Double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }

         
        [Display(Name ="pickup Name")]
        public string Picupname { get; set; }
        [Display(Name ="Phone Number")]
        public string Phonenumber{ get; set; }
        public string TransactionID{ get; set; }
    }
}
