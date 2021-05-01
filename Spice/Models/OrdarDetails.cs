using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrdarDetails
    {
        [Key]
        public int Id{ get; set; }

        public int OrdarId{ get; set; }
        [ForeignKey("OrdarId")]
        public virtual OrdarHedar OrdarHedar { get; set; }

        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        public int Count{ get; set; }

        public string Discrption{ get; set; }

        public string Name{ get; set; }

        public double Pric{ get; set; }



    }
}
