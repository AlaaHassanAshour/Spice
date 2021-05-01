using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Utilti
{
    public static class SD
    {
        public const string ManegerUser = "Maneger";
        public const string KithenUser = "Kithen";
        public const string FrontDiskUser = "FrontDisk";
        public const string CustmerEndUser = "Custmer";

        public const string ShoppingCartCount = "ShoppingCartCount";
        public const string ssCouponCode= "CouponCode";


        public const string StatusSubitmed= "Subitmed";
        public const string StatusInProccess= "Begin Prepared";
        public const string StatusReady= "Ready For Pickup";
        public const string StatusCompleted= "Completed";
        public const string StatusCancelled= "Cancelled";
        
        
        public const string PaymentStatusPending= "Pending";
        public const string PaymentStatusApproved= "Approved";
        public const string PaymentStatusRejected= "Rejected";


        public static double DiscountPrice(Coupon coupon , double ordarTotalOrigen)
        {
            if (coupon==null)
            {
                return Math.Round(ordarTotalOrigen, 2);
            }
            else
            {
                if (coupon.MinimumAmount>ordarTotalOrigen)
                {
                    return Math.Round(ordarTotalOrigen, 2);
                }
                else
                {
                    if (int.Parse(coupon.CoubonType)==(int)Coupon.ECoubonType.Dollar)
                    {
                        return Math.Round(ordarTotalOrigen-coupon.Discount, 2);
                    }
                    else
                    {
                        return Math.Round(ordarTotalOrigen-(ordarTotalOrigen*(coupon.Discount/100)), 2);
                    }
                }
            }
        }

    }
}
