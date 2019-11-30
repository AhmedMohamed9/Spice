using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Utility
{
    public class SD
    {
        public const string ManagerUser = "Manager";
        public const string KitchenUser = "Kitchen";
        public const string FrontDeskUser = "FrontDesk";
        public const string CustomerEnduser = "Customer";
        public const string sscouponcode = "sscouponcode";

        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Bieng Prepared";
        public const string StatusReady = "Ready For pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled= "Cancelled";

       


        public static double discount(Copuns coupon,double originalordertotal)
        {
            if (coupon == null)
            {
                return originalordertotal;
            }
            else
            {
                if (coupon.MinimumAmount>originalordertotal)
                {
                    return originalordertotal;
                }
                else
                {
                    if (Convert.ToInt32(coupon.CouponType) == (int)Copuns.EcouponType.Dollar)
                    {
                        return Math.Round(originalordertotal - coupon.Discount, 2);
                    }if (Convert.ToInt32(coupon.CouponType) == (int)Copuns.EcouponType.Percent)
                    {
                        return Math.Round(originalordertotal-(originalordertotal * coupon.Discount/100), 2);
                    }
                }
                return originalordertotal;
            }
        }
    }
}
