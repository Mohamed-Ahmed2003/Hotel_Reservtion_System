using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservtion_System.Entity
{
    public class Notification
    {
        [Key]
        public Guid id { get; set; }
        [ForeignKey("userID")]
        public User? user { get; set; }
        [StringLength(100)]
        public string? message { get; set; }
        [StringLength(10)]
        public string? status { get; set; }
    }
}
