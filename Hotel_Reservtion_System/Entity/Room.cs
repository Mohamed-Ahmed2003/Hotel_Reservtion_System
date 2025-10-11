using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservtion_System.Entity
{
    public class Room
    {
        [Key]
        public Guid id { get; set; }
        [StringLength(20)]
        public string? roomType { get; set; }
        [StringLength(10)]
        public string? roomCode { get; set; }
        [StringLength(20)]
        public bool availability { get; set; }
        public double? price { get; set; }
     
        [ForeignKey("branchID")]
        public Branch? branch { get; set; }
        public DateTime? cheakout { get; set; }
    }
}
