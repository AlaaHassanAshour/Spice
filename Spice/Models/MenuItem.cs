using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class MenuItem
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        public string Name{ get; set; }

        public string Decrption{ get; set; }

        public double Price{ get; set; }

        public string Imeg{ get; set; }

        public string Spicyness{ get; set; }

        public enum ESpicy{NA=0,NotSpicy=1,Spicy=2,VerySpicy=3}

        [Display(Name ="Category Name")]
        public int CategoryId{ get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category{ get; set; }
     

        [Display(Name = "SubCategory Name")]
        public int SubCategoryId{ get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

      
    }
}
