using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservtion_System.Entity
{
    public class Invoice
    {
        [Key]
        public Guid id { get; set; }
        [ForeignKey("bookID")]
        public Booking? booking { get; set; }
        public double? totalAmount { get; set; }
        public  double? discount { get; set; }=0;
        public  double? tax { get; set; }=0.1;
        public double? finalAmount { get; set; }
        [StringLength(10)]
        public string? status { get; set; }
        [ForeignKey("createdByid")]
        public User? createdBy { get; set; }
    }
}
