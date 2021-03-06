using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Coupon
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public string CoubonType{ get; set; }
        public enum ECoubonType{percnt=0,Dollar=1}
        [Required]
        public double Discount{ get; set; }
        [Required,Display(Name = "Minimum Amount")]
        public double MinimumAmount{ get; set; }
        public byte[] Pictsur{ get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive{ get; set; }

       
    }
}
