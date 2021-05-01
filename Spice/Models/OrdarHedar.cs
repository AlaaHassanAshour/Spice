using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spice.Models
{
    public class OrdarHedar
    {
        [Key]
        public int Id { get; set; }
      
        public string UserId { get; set; }
        [ForeignKey("UserId ")]
        public virtual ApplicationUser ApplicationUser { get; set; }
       
        [Required]
        public DateTime OrdarDate { get; set; }
        
        
        public double OrdarTotalOregnal { get; set; }
       
        [Required, DisplayFormat(DataFormatString = "{0:c}")]
        public double OrdarTotal { get; set; }
       
        [Required, Display(Name = "Pick Up Time")]
        public DateTime PickUpTime { get; set; }
      
        [Required, NotMapped]
        public DateTime PickUpDate { get; set; }
       
        [Display(Name = "Coupn Code")]
        public string CoupnCode { get; set; }
     
        public double CoupnCodeDiscount { get; set; }
        
        public string State { get; set; }
      
        public string PymentState { get; set; }
        
        public string Commant { get; set; }
       
        [Display(Name = "PickUp Name")]
        public string PickUpName { get; set; }
       
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
       
        public string TrasactionId { get; set; }
    }
}
