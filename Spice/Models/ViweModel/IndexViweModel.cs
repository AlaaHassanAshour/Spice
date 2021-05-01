using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViweModel
{
    public class IndexViweModel
    {
        public IEnumerable <Category> Categorys { get; set; }
        public IEnumerable <MenuItem >MenuItems { get; set; }
        public IEnumerable <Coupon> Coupons { get; set; }

    }
}
